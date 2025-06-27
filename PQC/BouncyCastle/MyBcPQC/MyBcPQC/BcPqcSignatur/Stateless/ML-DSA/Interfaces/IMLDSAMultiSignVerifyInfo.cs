


namespace michele.natale.BcPqcs;

/// <summary>
/// Interface to class MLDSAMultiSignVerifyInfo.
/// </summary>
public interface IMLDSAMultiSignVerifyInfo
{

  /// <summary>
  /// Returns KeyPair informations.
  /// </summary>
  /// <param name="mldsainfo">Desired sign information</param>
  /// <returns>Returns KeyPair informations.</returns>
  static abstract MlDsaKeyPairInfo ToMultiInfo(ReadOnlySpan<MLDSASignInfo> mldsainfo);

  /// <summary>
  /// Returns the multi-signature based on the desired parameters.
  /// </summary>
  /// <param name="info">Desired keypair information</param>
  /// <param name="message">Desired message</param>
  /// <returns>Returns as Bytes</returns>
  static abstract byte[] MultiSign(MlDsaKeyPairInfo info, ReadOnlySpan<byte> message);

  /// <summary>
  /// Verifies the multi-signature based on the desired parameters.
  /// </summary>
  /// <param name="info">Desired keypair information.</param>
  /// <param name="signature">The desired signatures of all signatories.</param>
  /// <param name="message">Desired message.</param>
  /// <returns>True, verifiy is ok, ortherwise false.</returns>
  static abstract bool MultiVerify(MlDsaKeyPairInfo info, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message);


  /// <summary>
  /// Returns the multi-signature based on the desired parameters.
  /// </summary>
  /// <param name="info">Desired keypair information.</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>Returns as bytes</returns>
  static abstract byte[] MultiSign(MlDsaKeyPairInfo info, string datapath);

  /// <summary>
  /// Verifies the multi-signature based on the desired parameters.
  /// </summary>
  /// <param name="info">Desired keypair information</param>
  /// <param name="signature">The desired signatures of all signatories.</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>true, verify is ok, ortherwise false.</returns>
  static abstract bool MultiVerify(MlDsaKeyPairInfo info, ReadOnlySpan<byte> signature, string datapath);
}
