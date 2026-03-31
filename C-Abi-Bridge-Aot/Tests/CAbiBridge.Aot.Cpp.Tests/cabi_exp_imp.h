#pragma once

#include <cstdint>


//#define EXP32 extern "C" __declspec(dllexport)
//#define IMP32 extern "C" __declspec(dllimport)
 

#if defined(_WIN32) || defined(_WIN64)
#define CABI_EXPORT extern "C" __declspec(dllexport)
#define CABI_IMPORT extern "C" __declspec(dllimport)
#else
#define CABI_EXPORT extern "C" __attribute__((visibility("default")))
#define CABI_IMPORT extern "C"
#endif

#ifdef CABI_BRIDGE_BUILD
#define CABI_API CABI_EXPORT
#else
#define CABI_API CABI_IMPORT
#endif