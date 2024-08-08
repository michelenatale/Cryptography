
using System.Reflection;

namespace michele.natale.LoginSystems.Services;

using Views;
using Models;
using Pointers;
using ViewModels;

partial class AppServices
{

  /// <summary>
  /// Replaces the currently integrated UserControl with the desired one. 
  /// </summary>
  /// <param name="frm">FrmMain</param>
  /// <param name="uc">Desired UserControl</param>
  /// <param name="vm_frm">Viewmodel-Main</param>
  public void SetFrmUserControl(Form frm, UserControl uc, VmMain vm_frm)
  {
    ClearAllUserControls(frm);  //First here for UC-Disposing
    SetProperty(frm, uc, vm_frm);
    ClearSetUserControl(frm, uc);
  }

  /// <summary>
  /// Dispose of the key available for the login process
  /// </summary>
  internal void DisposeKey()
  {
    if (PKey is null) return;
    if (PKey == UsIPtr<byte>.Empty) return;
    using var k = PKey;
  }

  /// <summary>
  /// Returns the Current UserControl
  /// </summary>
  /// <param name="frm">FramMain</param>
  /// <returns></returns>
  public UserControl CurrentUserControl(Form frm)
  {
    ArgumentNullException.ThrowIfNull(frm);

    var tuc = typeof(UserControl);
    foreach (var ctr in frm.Controls)
      if (ctr.GetType() == tuc ||
          ctr.GetType().BaseType == tuc ||
          ctr.GetType().BaseType?.BaseType == tuc)
        return (UserControl)ctr;
    return null!;
  }

  /// <summary>
  /// Returns the ViewModel-Main
  /// </summary>
  /// <param name="frm">FrmMain</param>
  /// <returns></returns>
  public VmMain ToFrmViewModel(Form frm)
  {
    var bs_frm = (BindingSource)frm.GetType().GetField(
      $"BsViewModel", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(frm)!;
    return (VmMain)bs_frm.DataSource;
  }

  /// <summary>
  /// Returns the current view model
  /// </summary>
  /// <param name="frm">FrmMain</param>
  /// <returns></returns>
  /// <exception cref="NotImplementedException"></exception>
  public VmBase CurrentViewModel(Form frm)
  {
    var uc = CurrentUserControl(frm);
    var bs_frm = (BindingSource)frm.GetType().GetField(
      $"BsViewModel", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(frm)!;
    var vm_main = (VmMain)bs_frm.DataSource;

    return uc switch
    {
      object obj when obj is UcMainEx => vm_main.VmUcMain,
      object obj when obj is UcLoginEx => vm_main.VmUcLogin,
      object obj when obj is UcRegistEx => vm_main.VmUcRegist,
      object obj when obj is UcPwChangeEx => vm_main.VmUcPwChange,
      object obj when obj is UcPwForgetEx => vm_main.VmUcPwForget,
      _ => throw new NotImplementedException(),
    };
  }

  /// <summary>
  /// Returns a new instance of the desired UserControl.
  /// </summary>
  /// <param name="cmd">FrmCommands</param>
  /// <returns></returns>
  public UserControl ToNewUserControl(FrmCommands cmd)
  {
    return cmd switch
    {
      FrmCommands.UcMain => new UcMainEx() { Dock = DockStyle.None },
      FrmCommands.UcLogin => new UcLoginEx() { Dock = DockStyle.None },
      FrmCommands.UcRegist => new UcRegistEx() { Dock = DockStyle.None },
      FrmCommands.UcPwChange => new UcPwChangeEx() { Dock = DockStyle.None },
      FrmCommands.UcPwForget => new UcPwForgetEx() { Dock = DockStyle.None },
      _ => new UserControl(),
    };
  }

  /// <summary>
  /// Sets the desired bindingsource with the desired parameters.
  /// </summary>
  /// <param name="frm">Fram Main</param>
  /// <param name="uc">Desired UserControl.</param>
  /// <param name="vm_frm">Main-ViewModel</param>
  private void SetProperty(Form frm, UserControl uc, VmMain vm_frm)
  {
    var propname = "PVmUc";

    var uc_name = uc.Name.Replace("Ex", string.Empty);
    var datamember = $"Vm{uc_name}";
    var bs_uc = (BindingSource)uc.GetType().GetProperty($"Bs{uc_name}",
      BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(uc)!;
    var bs_frm = (BindingSource)frm.GetType().GetField(
      $"BsViewModel", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(frm)!;

    if (uc is UcMainEx) bs_uc.DataSource = vm_frm.VmUcMain;
    else if (uc is UcLoginEx) bs_uc.DataSource = vm_frm.VmUcLogin;
    else if (uc is UcRegistEx) bs_uc.DataSource = vm_frm.VmUcRegist;
    else if (uc is UcPwChangeEx) bs_uc.DataSource = vm_frm.VmUcPwChange;
    else if (uc is UcPwForgetEx) bs_uc.DataSource = vm_frm.VmUcPwForget;

    for (var i = 0; i < frm.DataBindings.Count; i++)
      if (frm.DataBindings[i].PropertyName == propname)
        frm.DataBindings.Remove(frm.DataBindings[i]);

    frm.DataBindings.Add(new Binding(propname, bs_frm, datamember, true, DataSourceUpdateMode.OnPropertyChanged));
  }

  /// <summary>
  /// Removes and resets all UserControls in the FramMain.
  /// </summary>
  /// <param name="frm">FrmMain</param>
  public void ClearAllUserControls(Form frm)
  {
    ClearAllTypeControls(frm, typeof(UserControl));
  }

  /// <summary>
  /// Removes and resets all desired Types in the FramMain.
  /// </summary>
  /// <param name="frm">FrmMain</param>
  /// <param name="tuc">desired Types</param>
  public void ClearAllTypeControls(Form frm, Type tuc)
  {
    //With resetting the UserControl
    ArgumentNullException.ThrowIfNull(frm);
    frm.SuspendLayout();
    foreach (var ctr in frm.Controls)
    {
      if (ctr.GetType() == tuc ||
          ctr.GetType().BaseType == tuc ||
          ctr.GetType().BaseType?.BaseType == tuc)
      {
        using var c = (Control)ctr;
        c.Enabled = false;
        c.Dock = DockStyle.None;
        frm.Controls.Remove(c);
      }
    }
    frm.ResumeLayout(true);
  }

  /// <summary>
  /// Removes, resets and set the desired UserControls in the FramMain.
  /// </summary>
  /// <param name="frm">FrmMain</param>
  /// <param name="uc">desired UserControls</param>
  public void ClearSetUserControl(Form frm, UserControl uc)
  {
    ArgumentNullException.ThrowIfNull(uc, nameof(uc));
    ClearAllUserControls(frm);
    frm.SuspendLayout();
    frm.Controls.Add(uc);
    uc.Dock = DockStyle.Fill;
    uc.Enabled = true;
    frm.ResumeLayout(true);
  }

  /// <summary>
  /// Removes and resets the UserControls in the FramMain.
  /// </summary>
  /// <param name="frm">FrmMain</param>
  /// <param name="uc">desired UserControl</param>
  public void RemoveFrmUc(Form frm, UserControl uc)
  {
    //Without resetting the UserControl
    ArgumentNullException.ThrowIfNull(uc, nameof(uc));
    ArgumentNullException.ThrowIfNull(frm, nameof(frm));

    frm.SuspendLayout();
    foreach (var itm in frm.Controls)
    {
      if (itm is UserControl ctr && ReferenceEquals(itm, uc))
      {
        uc.Enabled = false;
        uc.Dock = DockStyle.None;
        frm.Controls.Remove(ctr);
      }
    }
    frm.ResumeLayout(true);
  }

  /// <summary>
  /// Sets the bindingsource in the desired UserControl.
  /// </summary>
  /// <param name="frm">FrmMain</param>
  /// <param name="uc">desired UserControl</param>
  /// <param name="property"></param>
  public void SetUcBinding(Form frm, UserControl uc, object property)
  {
    var uc_name = uc.Name.Replace("Ex", string.Empty);
    var bs_uc = (BindingSource)uc.GetType().GetProperty($"Bs{uc_name}",
      BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(uc)!;
    bs_uc.DataSource = property;
  }
}
