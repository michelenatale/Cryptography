#include "pch.h"

#include <random>
#include <chrono>
#include <string>
#include <vector>
#include <cstdint>
#include <iostream>
#include <fstream>
#include <iterator>

#include "bridge.h"
#include "fcp_type.h"
#include "compression_type.h"
#include "compression_level.h"
#include "crypto_utils_test.h"
#include "file_compress_package_test.h"


namespace michele::natale::Tests
{
  struct pack_list_ptr
  {
    uint8_t** fnamesptr;
    int32_t* flengthsptr;
    int32_t count;
  };

  fcp_type_t random_fcp_type_f();
  std::string rng_folder_name(int size);
  void finish_source(const char* srcfolder);
  compression_type_t random_compress_type_f();
  compression_level_t random_compress_level_f();
  void preparation_source(const char* srcfolder);
  pack_list_ptr to_pack_list_ptr(const char* packlist[], int count);
  bool file_equals_spec(const char* srcfolder, const char* destfolder);
  void copy_file(const char* sourcepath, const char* destpath, bool overwrite);
  void create_rng_folders(const char* basefolder, const char* files[], int file_count);
  bool file_equals_spec(const char* filelist[], int count, const char* outputfolder);


  static void test_pack_none_file()
  {
    auto start = std::chrono::high_resolution_clock::now();

    const char* outputfolder = "output";
    const char* archivepath = "test.fcp";

    int packlist_count = 4; //for this samples
    const char* packlist[] = { "data2.txt", "data3.txt", "data2.txt", "data3.txt" };

    compression_type_t compresstype = compression_type_t::None;

    std::string archiv_path_utf8(archivepath);
    auto [fnamesptr, fnameslengthsptr, count] =
      to_pack_list_ptr(packlist, packlist_count);

    int64_t totalfilesize = 0;
    int64_t totalcompresssize = 0;

    cerror_t err = pack_file_aot(
      (const uint8_t**)fnamesptr, (const int32_t*)fnameslengthsptr, count,
      (const uint8_t*)archiv_path_utf8.data(), (int32_t)archiv_path_utf8.size(),
      (uint8_t)compresstype,
      &totalfilesize, &totalcompresssize);
    assert_error(err);

    std::string output_folder_utf8(outputfolder);

    err = unpack_file_archiv_aot(
      (const uint8_t*)archiv_path_utf8.data(), (int32_t)archiv_path_utf8.size(),
      (const uint8_t*)output_folder_utf8.data(), (int32_t)output_folder_utf8.size());
    assert_error(err);

    if (!file_equals_spec(packlist, packlist_count, outputfolder))
      throw std::runtime_error("roundtrip");

    auto end = std::chrono::high_resolution_clock::now();
    double t = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << "test_pack_none_file_aot: ";

    auto [sum_size, fcnt] = sum_file_sizes(packlist, packlist_count);
    int64_t compress_size = file_size_fs(archivepath);

    std::cout << " t = " << t
      << "ms; file_count = " << fcnt
      << "; file_sizes = " << sum_size
      << "; compress_size = " << compress_size
      << "\n";
  }

  static void test_pack_none_bs_file()
  {
    auto start = std::chrono::high_resolution_clock::now();

    const char* outputfolder = "output";
    const char* archivepath = "test.fcp";

    const char* packlist[] = { "data2.txt", "data3.txt", "data2.txt", "data3.txt" };
    int packlist_count = 4;

    auto compresslevel = (uint8_t)random_compress_level_f();
    compression_type_t compresstype = compression_type_t::None;

    const uint8_t* archiv_path_utf8 = reinterpret_cast<const uint8_t*>(archivepath);
    int32_t archiv_path_len = (int32_t)strlen(archivepath);

    auto [fnamesptr, fnameslengthsptr, count] =
      to_pack_list_ptr(packlist, packlist_count);

    int64_t totalfilesize = 0;
    int64_t totalcompresssize = 0;

    cerror_t err = pack_file_bs_cl_aot(
      (const uint8_t**)fnamesptr, (const int32_t*)fnameslengthsptr, count,
      archiv_path_utf8, archiv_path_len,
      (uint8_t)compresstype, BUFFER_SIZE_DEFAULT, compresslevel,
      &totalfilesize, &totalcompresssize);
    assert_error(err);

    const uint8_t* output_folder_utf8 =
      reinterpret_cast<const uint8_t*>(outputfolder);
    int32_t output_folder_len = (int32_t)strlen(outputfolder);

    err = unpack_file_archiv_bs_aot(
      archiv_path_utf8,
      archiv_path_len,
      output_folder_utf8,
      output_folder_len,
      BUFFER_SIZE_DEFAULT);
    assert_error(err);

    if (!file_equals_spec(packlist, packlist_count, outputfolder))
      throw std::runtime_error("roundtrip");

    auto end = std::chrono::high_resolution_clock::now();
    double t = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << "test_pack_none_bs_file_aot: ";

    auto [sum_size, fcnt] = sum_file_sizes(packlist, packlist_count);
    int64_t compress_size = file_size_fs(archivepath);

    std::cout << " t = " << t
      << "ms; file_count = " << fcnt
      << "; file_sizes = " << sum_size
      << "; compress_size = " << compress_size
      << "\n";
  }


  static void test_pack_gzip_file()
  {
    auto start = std::chrono::high_resolution_clock::now();

    const char* outputfolder = "output";
    const char* archivepath = "test.fcp";

    const char* packlist[] = { "data2.txt", "data3.txt", "data2.txt", "data3.txt" };
    int packlist_count = 4;

    compression_type_t compresstype = compression_type_t::GZip;

    const uint8_t* archiv_path_utf8 = reinterpret_cast<const uint8_t*>(archivepath);
    int32_t archiv_path_len = (int32_t)strlen(archivepath);

    auto [fnamesptr, fnameslengthsptr, count] =
      to_pack_list_ptr(packlist, packlist_count);

    int64_t totalfilesize = 0;
    int64_t totalcompresssize = 0;

    cerror_t err = pack_file_aot(
      (const uint8_t**)fnamesptr, (const int32_t*)fnameslengthsptr, count,
      archiv_path_utf8, archiv_path_len,
      (uint8_t)compresstype,
      &totalfilesize, &totalcompresssize);
    assert_error(err);

    const uint8_t* output_folder_utf8 =
      reinterpret_cast<const uint8_t*>(outputfolder);
    int32_t output_folder_len = (int32_t)strlen(outputfolder);

    err = unpack_file_archiv_aot(
      archiv_path_utf8, archiv_path_len,
      output_folder_utf8, output_folder_len);
    assert_error(err);

    if (!file_equals_spec(packlist, packlist_count, outputfolder))
      throw std::runtime_error("roundtrip");

    auto end = std::chrono::high_resolution_clock::now();
    double t = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << "test_pack_gzip_file_aot: ";

    auto [sum_size, fcnt] = sum_file_sizes(packlist, packlist_count);
    int64_t compress_size = file_size_fs(archivepath);

    std::cout << " t = " << t
      << "ms; file_count = " << fcnt
      << "; file_sizes = " << sum_size
      << "; compress_size = " << compress_size
      << "\n";
  }

  static void test_pack_brotli_file()
  {
    auto start = std::chrono::high_resolution_clock::now();

    const char* outputfolder = "output";
    const char* archivepath = "test.fcp";

    int packlist_count = 4;
    const char* packlist[] = { "data2.txt", "data3.txt", "data2.txt", "data3.txt" };

    compression_type_t compresstype = compression_type_t::Brotli;
    const uint8_t* archiv_path_utf8 = reinterpret_cast<const uint8_t*>(archivepath);
    int32_t archiv_path_len = (int32_t)strlen(archivepath);

    auto [fnamesptr, fnameslengthsptr, count] =
      to_pack_list_ptr(packlist, packlist_count);

    int64_t totalfilesize = 0;
    int64_t totalcompresssize = 0;

    cerror_t err = pack_file_aot(
      (const uint8_t**)fnamesptr, (const int32_t*)fnameslengthsptr, count,
      archiv_path_utf8, archiv_path_len,
      (uint8_t)compresstype,
      &totalfilesize, &totalcompresssize);
    assert_error(err);

    const uint8_t* output_folder_utf8 =
      reinterpret_cast<const uint8_t*>(outputfolder);
    int32_t output_folder_len = (int32_t)strlen(outputfolder);

    err = unpack_file_archiv_aot(
      archiv_path_utf8, archiv_path_len,
      output_folder_utf8, output_folder_len);
    assert_error(err);

    if (!file_equals_spec(packlist, packlist_count, outputfolder))
      throw std::runtime_error("roundtrip");

    auto end = std::chrono::high_resolution_clock::now();
    double t = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << "test_pack_brotli_file_aot: ";

    auto [sum_size, fcnt] = sum_file_sizes(packlist, packlist_count);
    int64_t compress_size = file_size_fs(archivepath);

    std::cout << " t = " << t
      << "ms; file_count = " << fcnt
      << "; file_sizes = " << sum_size
      << "; compress_size = " << compress_size
      << "\n\n\n";
  }


  static void test_pack_none_archiv(const char* src_folder)
  {
    auto start = std::chrono::high_resolution_clock::now();

    const char* outputfolder = "output";
    const char* archivepath = "test.fcp";

    compression_type_t compresstype = compression_type_t::None;

    const uint8_t* src_folder_utf8 =
      reinterpret_cast<const uint8_t*>(src_folder);
    int32_t src_folder_len = (int32_t)strlen(src_folder);

    const uint8_t* archive_path_utf8 =
      reinterpret_cast<const uint8_t*>(archivepath);
    int32_t archive_path_len = (int32_t)strlen(archivepath);

    int64_t totalfilesize = 0;
    int64_t totalcompresssize = 0;

    cerror_t err = pack_archiv_aot(
      src_folder_utf8, src_folder_len,
      archive_path_utf8, archive_path_len,
      (uint8_t)compresstype,
      &totalfilesize, &totalcompresssize);
    assert_error(err);

    const uint8_t* output_folder_utf8 =
      reinterpret_cast<const uint8_t*>(outputfolder);
    int32_t output_folder_len = (int32_t)strlen(outputfolder);

    err = unpack_file_archiv_aot(
      archive_path_utf8, archive_path_len,
      output_folder_utf8, output_folder_len);
    assert_error(err);

    if (!file_equals_spec(src_folder, outputfolder))
      throw std::runtime_error("roundtrip");

    auto end = std::chrono::high_resolution_clock::now();
    double t = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << "test_pack_none_archiv_aot: ";

    auto [sum_size, fcnt] = sum_file_sizes_folder(src_folder);
    int64_t compress_size = file_size_fs(archivepath);

    std::cout << " t = " << t
      << "ms; file_count = " << fcnt
      << "; file_sizes = " << sum_size
      << "; compress_size = " << compress_size
      << "\n";
  }

  static void test_pack_gzip_archiv(const char* srcfolder)
  {
    auto start = std::chrono::high_resolution_clock::now();

    const char* outputfolder = "output";
    const char* archivepath = "test.fcp";

    compression_type_t compresstype = compression_type_t::GZip;

    const uint8_t* src_folder_utf8 =
      reinterpret_cast<const uint8_t*>(srcfolder);
    int32_t src_folder_len = (int32_t)strlen(srcfolder);

    const uint8_t* archive_path_utf8 =
      reinterpret_cast<const uint8_t*>(archivepath);
    int32_t archive_path_len = (int32_t)strlen(archivepath);

    int64_t totalfilesize = 0;
    int64_t totalcompresssize = 0;

    cerror_t err = pack_archiv_aot(
      src_folder_utf8, src_folder_len,
      archive_path_utf8, archive_path_len,
      (uint8_t)compresstype,
      &totalfilesize, &totalcompresssize);
    assert_error(err);

    const uint8_t* output_folder_utf8 =
      reinterpret_cast<const uint8_t*>(outputfolder);
    int32_t output_folder_len = (int32_t)strlen(outputfolder);

    err = unpack_file_archiv_aot(
      archive_path_utf8, archive_path_len,
      output_folder_utf8, output_folder_len);
    assert_error(err);

    if (!file_equals_spec(srcfolder, outputfolder))
      throw std::runtime_error("roundtrip");

    auto end = std::chrono::high_resolution_clock::now();
    double t = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << "test_pack_gzip_archiv_aot: ";
    auto [sum_size, fcnt] = sum_file_sizes_folder(srcfolder);
    int64_t compress_size = file_size_fs(archivepath);

    std::cout << " t = " << t
      << "ms; file_count = " << fcnt
      << "; file_sizes = " << sum_size
      << "; compress_size = " << compress_size
      << "\n";
  }

  static void test_pack_brotli_archiv(const char* srcfolder)
  {
    auto start = std::chrono::high_resolution_clock::now();

    const char* outputfolder = "output";
    const char* archivepath = "test.fcp";

    compression_type_t compresstype = compression_type_t::Brotli;

    const uint8_t* src_folder_utf8 =
      reinterpret_cast<const uint8_t*>(srcfolder);
    int32_t src_folder_len = (int32_t)strlen(srcfolder);

    const uint8_t* archive_path_utf8 =
      reinterpret_cast<const uint8_t*>(archivepath);
    int32_t archive_path_len = (int32_t)strlen(archivepath);

    int64_t totalfilesize = 0;
    int64_t totalcompresssize = 0;

    cerror_t err = pack_archiv_aot(
      src_folder_utf8, src_folder_len,
      archive_path_utf8, archive_path_len,
      (uint8_t)compresstype,
      &totalfilesize, &totalcompresssize);
    assert_error(err);

    const uint8_t* output_folder_utf8 =
      reinterpret_cast<const uint8_t*>(outputfolder);
    int32_t output_folder_len = (int32_t)strlen(outputfolder);

    err = unpack_file_archiv_aot(
      archive_path_utf8,
      archive_path_len,
      output_folder_utf8,
      output_folder_len);
    assert_error(err);

    if (!file_equals_spec(srcfolder, outputfolder))
      throw std::runtime_error("roundtrip");

    auto end = std::chrono::high_resolution_clock::now();
    double t = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << "test_pack_brotli_archiv_aot: ";

    auto [sum_size, fcnt] = sum_file_sizes_folder(srcfolder);
    int64_t compress_size = file_size_fs(archivepath);

    std::cout << " t = " << t
      << "ms; file_count = " << fcnt
      << "; file_sizes = " << sum_size
      << "; compress_size = " << compress_size
      << "\n\n";
  }

  fcp_type_t random_fcp_type_f()
  {
    auto values = to_fcp_type();

    // call NativeAOT RNG 
    cerror_t err;
    auto rng = next_crypto_int32_max_aot(
      (int)values.size(), &err);
    assert_error(err);

    return values[rng];
  }

  compression_type_t random_compress_type_f()
  {
    auto values = to_compression_type();

    // call NativeAOT RNG 
    cerror_t err;
    auto rng = next_crypto_int32_max_aot(
      (int)values.size(), &err);
    assert_error(err);

    return values[rng];
  }

  compression_level_t random_compress_level_f()
  {
    auto values = to_compression_level();

    // call NativeAOT RNG 
    cerror_t err;
    auto rng = next_crypto_int32_max_aot(
      (int)values.size(), &err);
    assert_error(err);

    return values[rng];
  }
  static pack_list_ptr to_pack_list_ptr(
    const char* packlist[], int count)
  {
    if (count == 0)
      return { nullptr, nullptr, 0 };

    uint8_t** names = (uint8_t**)malloc(count * sizeof(uint8_t*));
    int32_t* lengths = (int32_t*)malloc(count * sizeof(int32_t));

    for (int i = 0; i < count; i++)
    {
      const char* s = packlist[i] ? packlist[i] : "";
      size_t len = strlen(s);

      uint8_t* str_ptr = (uint8_t*)malloc(len + 1);
      memcpy(str_ptr, s, len);
      str_ptr[len] = 0;

      names[i] = str_ptr;
      lengths[i] = (int32_t)len;
    }

    return { names, lengths, count };
  }

  static bool file_equals_spec(
    const char* filelist[], int count, const char* outputfolder)
  {
    for (int i = 0; i < count; i++)
    {
      const char* left = filelist[i];
      std::string right = std::string(outputfolder) + "/" + left;
      const uint8_t* left_utf8 = reinterpret_cast<const uint8_t*>(left);
      int32_t left_len = (int32_t)strlen(left);

      const uint8_t* right_utf8 = reinterpret_cast<const uint8_t*>(right.c_str());
      int32_t right_len = (int32_t)right.size();

      cerror_t err;
      bool eq = equal_files_aot(
        left_utf8, left_len,
        right_utf8, right_len, &err);
      assert_error(err);

      if (!eq)
        return false;
    }

    return true;
  }

   bool file_equals_spec(const char* srcfolder, const char* destfolder)
  {
    std::vector<std::string> left;
    std::vector<std::string> right;

    for (auto& p : std::filesystem::recursive_directory_iterator(srcfolder))
      if (p.is_regular_file())
        left.push_back(p.path().string());

    for (auto& p : std::filesystem::recursive_directory_iterator(destfolder))
      if (p.is_regular_file())
        right.push_back(p.path().string());

    std::sort(left.begin(), left.end());
    std::sort(right.begin(), right.end());

    if (left.size() != right.size())
      return false;

    for (size_t i = 0; i < left.size(); i++)
    {
      const uint8_t* left_utf8 = reinterpret_cast<const uint8_t*>(left[i].c_str());
      int32_t left_len = (int32_t)left[i].size();

      const uint8_t* right_utf8 = reinterpret_cast<const uint8_t*>(right[i].c_str());
      int32_t right_len = (int32_t)right[i].size();

      cerror_t err;
      bool eq = equal_files_aot(
        left_utf8, left_len,
        right_utf8, right_len,
        &err);

      assert_error(err);

      if (!eq)
        return false;
    }

    return true;
  }

   std::string rng_folder_name(int size)
  {
    static const char hex[] = "0123456789abcdef";

    std::mt19937_64 rng(std::random_device{}());
    std::uniform_int_distribution<int> pick(0, 15);

    std::string s;
    s.reserve(size);

    for (int i = 0; i < size; i++)
      s.push_back(hex[pick(rng)]);

    return s;
  }


   void copy_file(const char* sourcepath, const char* destpath, bool overwrite)
  {
    if (!std::filesystem::exists(sourcepath))
      throw std::runtime_error("source file not found");

    if (overwrite && std::filesystem::exists(destpath))
      std::filesystem::remove(destpath);

    std::ifstream in(sourcepath, std::ios::binary);
    std::ofstream out(destpath, std::ios::binary);

    out << in.rdbuf();
  }

   void create_rng_folders(const char* basefolder, const char* files[], int file_count)
  {
    std::mt19937_64 rng(std::random_device{}());
    std::uniform_int_distribution<int> pick(0, file_count - 1);
    std::uniform_real_distribution<double> prob(0.0, 1.0);

    if (std::filesystem::exists(basefolder))
      std::filesystem::remove_all(basefolder);

    std::filesystem::create_directory(basefolder);

    const char* file = files[pick(rng)];
    std::string dest = std::string(basefolder) + "/" + file;
    copy_file(file, dest.c_str(), true);

    for (int i = 0; i < 3; i++)
    {
      std::string subroot = std::string(basefolder) + "/" + rng_folder_name(8);
      std::filesystem::create_directory(subroot);

      std::string current = subroot;
      file = files[pick(rng)];
      dest = current + "/" + file;
      copy_file(file, dest.c_str(), true);

      for (int depth = 0; depth < 3; depth++)
      {
        current += "/" + rng_folder_name(8);
        std::filesystem::create_directory(current);

        int c = pick(rng) + 1;
        for (int j = 0; j < c; j++)
        {
          if (prob(rng) < 0.95)
          {
            file = files[pick(rng)];
            dest = current + "/" + file;
            copy_file(file, dest.c_str(), true);
          }
        }
      }
    }
  }



   void preparation_source(const char* srcfolder)
  {
    std::cout << "A SourceFolder with many files and directories is created.\n\n";

    const char* packlist[] = { "data.txt", "data2.txt", "data3.txt" };
    create_rng_folders(srcfolder, packlist, 3);
  }

   void finish_source(const char* srcfolder)
  {
    std::cout << "The source directory is deleted again.\n\n";

    if (std::filesystem::exists(srcfolder))
      std::filesystem::remove_all(srcfolder);
  }


  void start_file_compress_package(int rounds)
  {
    test_pack_none_file();
    test_pack_none_bs_file();
    test_pack_gzip_file();
    test_pack_brotli_file();

    const char* src_folder = "sourcefolder";
    preparation_source(src_folder);
    test_pack_none_archiv(src_folder);
    test_pack_gzip_archiv(src_folder);
    test_pack_brotli_archiv(src_folder);
    finish_source(src_folder);

    std::cout << "\n";
  }
}