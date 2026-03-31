#include "pch.h"

#include <openssl/sha.h>   
#include <openssl/evp.h>  // TODO: später entfernen, wenn eigene Engine fertig


#include <fstream>
#include <cstdint>
#include <vector>
#include <string>
#include <random>
#include <chrono>
#include <iostream>
#include <cassert>
#include <future>

#include "cerror.h"
#include "crypto_utils_test.h"

std::future<void> set_rng_file_data_async(const std::string& filename, int size)
{
  return std::async(std::launch::async, [=]() {
    set_rng_file_data(filename, size);
  });
}

void set_rng_file_data(const std::string& filename, int size)
{
  std::ofstream fsout(filename, std::ios::binary | std::ios::trunc);

  int length = size < 1024 * 1024 ? size : 1024 * 1024;

  while (length > 0)
  {
    auto data = rng_bytes(length);
    fsout.write(reinterpret_cast<const char*>(data.data()), data.size());

    size -= length;
    length = size < 1024 * 1024 ? size : 1024 * 1024;
  }
}

// Hilfsfunktion: kopiert aus void* + length in vector<uint8_t>
std::vector<uint8_t> to_bytes(void* ptr, int length)
{
  uint8_t* p = static_cast<uint8_t*>(ptr);
  return std::vector<uint8_t>(p, p + length);
}

// Hilfsfunktion: Fehler prüfen
void assert_error(cerror_t err)
{
  if (err.error_code != (int)cerror_code_t::Ok)
  {
    std::cerr << "Crypto error\n";
    std::abort();
  }
}



// ************ ************ ************ ************ 
// ************ ************ ************ ************ 


 std::vector<uint8_t> rng_bytes(size_t size)
{
  std::vector<uint8_t> v(size);
  std::random_device rd;
  for (auto& b : v) b = static_cast<uint8_t>(rd());
  return v;
}
  

std::vector<uint8_t> sha256_file(const std::string& filename)
{
  std::ifstream file(filename, std::ios::binary);
  if (!file)
    return {};

  EVP_MD_CTX* ctx = EVP_MD_CTX_new();
  EVP_DigestInit_ex(ctx, EVP_sha256(), nullptr);

  std::vector<char> buffer(1024 * 1024);

  while (file.good())
  {
    file.read(buffer.data(), buffer.size());
    std::streamsize bytes = file.gcount();
    if (bytes > 0)
      EVP_DigestUpdate(ctx, buffer.data(), bytes);
  }

  std::vector<uint8_t> hash(EVP_MAX_MD_SIZE);
  unsigned int hash_len = 0;

  EVP_DigestFinal_ex(ctx, hash.data(), &hash_len);
  EVP_MD_CTX_free(ctx);

  hash.resize(hash_len);
  return hash;
}

bool file_equals(const std::string& leftfile, const std::string& rightfile)
{
  auto left = sha256_file(leftfile);   // TODO: später ersetzen
  auto right = sha256_file(rightfile);  // TODO: später ersetzen

  return left == right;
}