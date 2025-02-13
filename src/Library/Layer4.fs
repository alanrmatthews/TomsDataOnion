namespace TomsDataOnion.Library


module Layer4 =
    let ipv4ChecksumCorrect (bytes: byte[], i: int) =
        let sum =
            uint32 (uint16 bytes[i + 0] <<< 8 ||| uint16 bytes[i + 1])
            + uint32 (uint16 bytes[i + 2] <<< 8 ||| uint16 bytes[i + 3])
            + uint32 (uint16 bytes[i + 4] <<< 8 ||| uint16 bytes[i + 5])
            + uint32 (uint16 bytes[i + 6] <<< 8 ||| uint16 bytes[i + 7])
            + uint32 (uint16 bytes[i + 8] <<< 8 ||| uint16 bytes[i + 9])
            + uint32 (uint16 bytes[i + 12] <<< 8 ||| uint16 bytes[i + 13])
            + uint32 (uint16 bytes[i + 14] <<< 8 ||| uint16 bytes[i + 15])
            + uint32 (uint16 bytes[i + 16] <<< 8 ||| uint16 bytes[i + 17])
            + uint32 (uint16 bytes[i + 18] <<< 8 ||| uint16 bytes[i + 19])

        let carryAddition = uint16 ((sum &&& 0xFFFFu) + (sum >>> 16))
        let not = ~~~carryAddition
        let checksum = uint32 (uint16 bytes[i + 10] <<< 8 ||| uint16 bytes[i + 11])
        ~~~carryAddition = (uint16 bytes[i + 10] <<< 8 ||| uint16 bytes[i + 11])


    let Peel (payload: string) =
        let bytes = Layer0.Ascii85DecodeBytes payload

        let sourceIPValid (i: int) =
            bytes[i + 12] = 10uy
            && bytes[i + 13] = 0uy
            && bytes[i + 14] = 0uy
            && bytes[i + 15] = 10uy

        let destinationIPValid (i: int) =
            bytes[i + 16] = 10uy
            && bytes[i + 17] = 0uy
            && bytes[i + 18] = 0uy
            && bytes[i + 19] = 200uy

        "TODO"
