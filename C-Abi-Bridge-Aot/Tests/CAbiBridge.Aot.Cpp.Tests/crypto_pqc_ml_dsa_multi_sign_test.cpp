#include "pch.h"

#include <iostream>
#include <vector>
#include <chrono>
#include <cstdlib>
#include <cstring>
#include <cstdio>

#include "bridge.h"
#include "cerror.h"
#include "variables.h"
#include "usi_ptr_t.h"
#include "native_buffer.h"
#include "crypto_utils_test.h"
#include "native_array_builder.h"
#include "crypto_pqc_ml_dsa_file_test.h"
#include "crypto_pqc_ml_dsa_utils_test.h"
#include "crypto_pqc_ml_dsa_param_test.h"
#include "crypto_pqc_ml_dsa_multi_sign_test.h"


namespace michele::natale::Tests
{

  void test_pqc_ml_dsa_multi_sign_kpf(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_multi_sign_kpf_aot: ";

    std::string folder = "keypairs";

    uint64_t sizetotal = 0;
    int64_t signercounttotal = 0;

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int r = 0; r < rounds; ++r)
    {
      int signers = rng_int(3, 16);
      signercounttotal += signers;

      // Signer names
      std::vector<std::string> names;
      for (int i = 0; i < signers; ++i)
        names.push_back("Signer_" + std::to_string(i));

      // Random message
      int size = rng_int(10, 126);
      auto message = rng_bytes(size);
      sizetotal += size;

      // Create + Save KeyPairs
      auto dict = create_ml_dsa_key_pair_and_save(names, folder);

      // Load KeyPairs
      auto kppis = load_native_ml_dsa_key_pair_all(folder);

      // Extract + Sort
      auto infos = extract_data_and_sort(kppis, dict, message, "Project");

      // Build native arrays
      native_array_builder guid(infos.size());
      native_array_builder sign(infos.size());
      native_array_builder pubk(infos.size());
      native_array_builder signernames(infos.size());
      native_array_builder projectnames(infos.size());

      uint8_t* sign_algo = to_malloc_uint8((int)infos.size());
      uint8_t* sign_algo_param = to_malloc_uint8((int)infos.size());

      for (size_t i = 0; i < infos.size(); ++i)
      {
        guid.set(i, infos[i].signer_id);
        sign.set(i, infos[i].signature);
        pubk.set(i, infos[i].public_key);
        signernames.set(i, infos[i].signer_name);
        projectnames.set(i, infos[i].project_name);

        sign_algo[i] = infos[i].pqc_sign_algo;
        sign_algo_param[i] = infos[i].pqc_sign_algo_param;
      }

      native_buffer msg(message);
      int multi_sign_len, multi_priv_len, multi_pub_len;
      uint8_t* multi_sign_ptr, * multi_priv_ptr, * multi_pub_ptr;

      cerror_t err = pqc_mldsa_multi_sign_aot(
        msg.ptr(), msg.len(),

        guid.ptrs(), guid.lens(), (int)guid.count(),
        sign.ptrs(), sign.lens(), (int)sign.count(),
        pubk.ptrs(), pubk.lens(), (int)pubk.count(),
        signernames.ptrs(), signernames.lens(), (int)signernames.count(),
        projectnames.ptrs(), projectnames.lens(), (int)projectnames.count(),

        sign_algo, (int)infos.size(),
        sign_algo_param, (int)infos.size(),

        &multi_sign_ptr, &multi_sign_len,
        &multi_priv_ptr, &multi_priv_len,
        &multi_pub_ptr, &multi_pub_len);
      assert_error(err);

      std::vector<uint8_t> multi_priv(multi_priv_ptr, multi_priv_ptr + multi_priv_len);
      std::vector<uint8_t> multi_pub(multi_pub_ptr, multi_pub_ptr + multi_pub_len);
      std::vector<uint8_t> multi_sign(multi_sign_ptr, multi_sign_ptr + multi_sign_len);

      free_buffer_aot(multi_pub_ptr);
      free_buffer_aot(multi_sign_ptr);
      free_buffer_aot(multi_priv_ptr);
      free(sign_algo); free(sign_algo_param);

      // Verify
      uint8_t ok = pqc_mldsa_verify_aot(
        message.data(), (int)message.size(),
        multi_pub.data(), (int)multi_pub.size(),
        multi_sign.data(), (int)multi_sign.size(),
        &err);
      assert_error(err);

      //What happens next:

      //Each signer receives the complete package(MlDsaSignerInfo), which includes
      //the message or message file, as well as the complete multi-signature data:
      //the multi-private key, multi-public key, and multi-MlDsa algorithm.

      //The MlDsaSignerInfo contains the GUID and name for each signer, the project
      //name, as well as each signer’s public key, signature, and the MlDSA algorithm
      //used.

      //This ensures that the process cannot be tampered with. Every single signature
      //can be traced back, and it must be ensured that the key pair remains available
      //for future use so that the relevant verifications can be reviewed and reconfirmed.
      //This could also be ensured by a neutral public authority that retains a complete
      //set of keys.


      //Wie geht es weiter:

      //Jeder Signer kriegt das komplette Packet (MlDsaSignerInfo), mit der Message bzw.
      //Messagedatei, sowie die kompletten Multi-Sign-Daten, also Multi-PrivKey,
      //Multi-PubKey, Multi-MlDsa-Algo.

      //Die MlDsaSignerInfo beinhaltet die Guid wie auch den Namen zu jeden Signer,
      //den Projektnamen, wie auch jeden PublicKey, jede Signatur und jeden verwendeten
      //mldsa-algorithm der Signer.

      //Somit ist gewährleistet, das keine Manipulation des Verfahrens gemacht werden kann.
      //Jede einzelne Signierung kann zurückverfolgt werden, wobei sichergestellt werden
      //muss, das der KeyPair - Schlüsselsatz auch für spätere Zwecke noch vorliegt, um
      //die entsprechenden Prüfungen nachzuvollziehen und nochmals sicherzustellen. Dies
      //könnte man auch mit einer neutralen Öffentlichen Instanz gewährleisten, die die
      //Wahrung einens kompletten Satzen für sich behält.

      if (!ok)
        throw std::runtime_error("MultiSign verify failed");

      if (r % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    double avg_kb = ((double)sizetotal / rounds);
    double avg_signers = (double)signercounttotal / rounds;

    std::cout << " rounds = " << rounds
      << "; signers = " << avg_signers
      << "; size = " << avg_kb
      << "; t = " << ms << "ms"
      << "; td = " << (ms / (double)rounds) << "ms\n";
  }


  void test_pqc_ml_dsa_multi_sign_file_kpf(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_multi_sign_file_kpf_aot: ";

    if (rounds < 10) rounds = 10;

    std::string srcfile = "data";
    std::string folder = "keypairs";
    std::string projectname = "Project ML-DSA-Multi-Sign-File-Test";

    uint64_t filesizetotal = 0;
    int64_t signercounttotal = 0;

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int r = 0; r < rounds; ++r)
    {
      // Anzahl Signer
      int signers = rng_int(3, 16);
      signercounttotal += signers;

      // Signer-Namen
      std::vector<std::string> signernames;
      for (int i = 0; i < signers; ++i)
        signernames.push_back("Signer_" + std::to_string(i));

      // RNG-Testfile erzeugen
      int max = (1 << 20) + 1024;
      int size = rng_int(1000, max);
      set_rng_file_data(srcfile, size);
      filesizetotal += size;

      // KeyPairs erzeugen + speichern
      auto dict = create_ml_dsa_key_pair_and_save(signernames, folder);

      // KeyPairs laden
      auto kppis = load_native_ml_dsa_key_pair_all(folder);

      // SignerInfos extrahieren + sortieren
      auto signerinfos = extract_data_and_sort(kppis, dict, srcfile, projectname);

      // Native Arrays bauen
      native_array_builder guid(signerinfos.size());
      native_array_builder sign(signerinfos.size());
      native_array_builder pubk(signerinfos.size());
      native_array_builder signer_names(signerinfos.size());
      native_array_builder projectnames(signerinfos.size());

      uint8_t* sign_algo = to_malloc_uint8((int)signerinfos.size());
      uint8_t* sign_algo_param = to_malloc_uint8((int)signerinfos.size());

      for (size_t i = 0; i < signerinfos.size(); ++i)
      {
        guid.set(i, signerinfos[i].signer_id);
        sign.set(i, signerinfos[i].signature);
        pubk.set(i, signerinfos[i].public_key);
        signer_names.set(i, signerinfos[i].signer_name);
        projectnames.set(i, signerinfos[i].project_name);

        sign_algo[i] = signerinfos[i].pqc_sign_algo;
        sign_algo_param[i] = signerinfos[i].pqc_sign_algo_param;
      }

      // Dateiname → NativeBuffer
      std::vector<uint8_t> fname(srcfile.begin(), srcfile.end());
      native_buffer file_name(fname);

      // MultiSignFile
      uint8_t* multi_sign_ptr, * multi_priv_ptr, * multi_pub_ptr;
      int multi_sign_len, multi_priv_len, multi_pub_len;

      cerror_t err = pqc_mldsa_multi_sign_file_aot(
        file_name.ptr(), file_name.len(),

        guid.ptrs(), guid.lens(), (int)guid.count(),
        sign.ptrs(), sign.lens(), (int)sign.count(),
        pubk.ptrs(), pubk.lens(), (int)pubk.count(),
        signer_names.ptrs(), signer_names.lens(), (int)signer_names.count(),
        projectnames.ptrs(), projectnames.lens(), (int)projectnames.count(),

        sign_algo, (int)signerinfos.size(),
        sign_algo_param, (int)signerinfos.size(),

        &multi_sign_ptr, &multi_sign_len,
        &multi_priv_ptr, &multi_priv_len,
        &multi_pub_ptr, &multi_pub_len);
      assert_error(err);

      std::vector<uint8_t> multi_sign(multi_sign_ptr, multi_sign_ptr + multi_sign_len);
      std::vector<uint8_t> multi_pub(multi_pub_ptr, multi_pub_ptr + multi_pub_len);

      free_buffer_aot(multi_sign_ptr);
      free_buffer_aot(multi_priv_ptr);
      free_buffer_aot(multi_pub_ptr);
      free(sign_algo); free(sign_algo_param);

      // VerifyFile 
      uint8_t ok = pqc_mldsa_verify_file_aot(
        file_name.ptr(), (int)file_name.len(),
        multi_pub.data(), (int)multi_pub.size(),
        multi_sign.data(), (int)multi_sign.size(),
        &err);
      assert_error(err);


      //What happens next:

      //Each signer receives the complete package(MlDsaSignerInfo), which includes
      //the message or message file, as well as the complete multi-signature data:
      //the multi-private key, multi-public key, and multi-MlDsa algorithm.

      //The MlDsaSignerInfo contains the GUID and name for each signer, the project
      //name, as well as each signer’s public key, signature, and the MlDSA algorithm
      //used.

      //This ensures that the process cannot be tampered with. Every single signature
      //can be traced back, and it must be ensured that the key pair remains available
      //for future use so that the relevant verifications can be reviewed and reconfirmed.
      //This could also be ensured by a neutral public authority that retains a complete
      //set of keys.


      //Wie geht es weiter:

      //Jeder Signer kriegt das komplette Packet (MlDsaSignerInfo), mit der Message bzw.
      //Messagedatei, sowie die kompletten Multi-Sign-Daten, also Multi-PrivKey,
      //Multi-PubKey, Multi-MlDsa-Algo.

      //Die MlDsaSignerInfo beinhaltet die Guid wie auch den Namen zu jeden Signer,
      //den Projektnamen, wie auch jeden PublicKey, jede Signatur und jeden verwendeten
      //mldsa-algorithm der Signer.

      //Somit ist gewährleistet, das keine Manipulation des Verfahrens gemacht werden kann.
      //Jede einzelne Signierung kann zurückverfolgt werden, wobei sichergestellt werden
      //muss, das der KeyPair - Schlüsselsatz auch für spätere Zwecke noch vorliegt, um
      //die entsprechenden Prüfungen nachzuvollziehen und nochmals sicherzustellen. Dies
      //könnte man auch mit einer neutralen Öffentlichen Instanz gewährleisten, die die
      //Wahrung einens kompletten Satzen für sich behält.

      if (!ok)
        throw std::runtime_error("MultiSignFile verify failed");

      if (r % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    double avg_signers = (double)signercounttotal / rounds;
    double avg_kb = ((double)filesizetotal / rounds) / 1024.0;

    std::cout << " rounds = " << rounds
      << "; signers = " << avg_signers
      << "; filesize = " << avg_kb << "kb"
      << "; t = " << ms << "ms"
      << "; td = " << (ms / (double)rounds) << "ms\n\n";
  }

}