# 💻 Using the C ABI from C

This guide shows how to call the C-Abi-Bridge-Aot functions from plain C.

## Include the Header

```
#include "bridge.h"
```

## Example: NextCryptoInt32Aot

```
int32_t value = 0;
CError err = next_crypto_int32_aot(&value);

if (err.error_code == 0) {
    printf("Value: %d\n", value);
}
```

## Example: RNG

```
int32_t* arr = NULL;
CError err = rng_crypto_int32_aot(128, &arr);

if (err.error_code == 0) {
    for (int i = 0; i < 128; i++)
        printf("%d\n", arr[i]);
}

free_buffer(arr);
```

## Example: AES-GCM

```
uint8_t* ciphertext = NULL;
CError err = aes_gcm_encrypt_aot(key, nonce, data, data_len, &ciphertext);

free_buffer(ciphertext);
```

