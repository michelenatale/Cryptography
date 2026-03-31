#pragma once

std::string encode64(const uint8_t* data, size_t length);

std::string hex_str_lower(const uint8_t* data, size_t length);
std::string hex_str_upper(const uint8_t* data, size_t length);