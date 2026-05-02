
#include "pch.h"

#include "compress_test.h"
#include "compress_gzip_brotli_test.h"
#include "file_compress_package_test.h"

namespace michele::natale::Tests
{

  void start_compress_native(int rounds)
  {
    start_compress_gzip_brotli(rounds);
    start_file_compress_package(rounds);
  }

}