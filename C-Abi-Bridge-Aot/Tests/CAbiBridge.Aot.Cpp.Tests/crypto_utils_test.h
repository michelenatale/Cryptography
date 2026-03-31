#pragma once 

#include <vector>
#include <string>
#include <future>
#include <iostream>

#include "cerror.h"


void assert_error(cerror_t err);
std::vector<uint8_t> rng_bytes(size_t size);
std::vector<uint8_t> to_bytes(void* ptr, int length);
void set_rng_file_data(const std::string& filename, int size);
bool file_equals(const std::string& leftfile, const std::string& rightfile);
std::future<void> set_rng_file_data_async(const std::string& filename, int size);

