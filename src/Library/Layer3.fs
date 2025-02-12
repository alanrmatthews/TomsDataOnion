namespace TomsDataOnion.Library


module Layer3 =
    let Peel (payload: string) =
        let bytes = Layer0.Ascii85DecodeBytes payload

        let key =
            [| bytes[0] ^^^ byte '='
               bytes[1] ^^^ byte '='
               bytes[2] ^^^ byte '['
               bytes[3] ^^^ byte ' '
               bytes[4] ^^^ byte 'L'
               bytes[5] ^^^ byte 'a'
               bytes[6] ^^^ byte 'y'
               bytes[7] ^^^ byte 'e'
               bytes[8] ^^^ byte 'r'
               bytes[9] ^^^ byte ' '
               bytes[10] ^^^ byte '4'
               bytes[11] ^^^ byte '/'
               bytes[12] ^^^ byte '6'
               bytes[13] ^^^ byte ':'
               bytes[14] ^^^ byte ' '
               bytes[47] ^^^ byte '='
               bytes[48] ^^^ byte '='
               bytes[49] ^^^ byte '='
               bytes[50] ^^^ byte '='
               bytes[51] ^^^ byte '='
               bytes[52] ^^^ byte '='
               bytes[53] ^^^ byte '='
               bytes[54] ^^^ byte '='
               bytes[55] ^^^ byte '='
               bytes[56] ^^^ byte '='
               bytes[57] ^^^ byte '='
               bytes[58] ^^^ byte '='
               bytes[59] ^^^ byte '='
               bytes[60] ^^^ 10uy // Newline
               bytes[61] ^^^ 10uy // Newline
               bytes[30] ^^^ byte ' '
               bytes[31] ^^^ byte ']' |]

        let decodedBytes =
            bytes |> Seq.mapi (fun i b -> b ^^^ key[i % key.Length]) |> Seq.toArray

        System.Text.Encoding.ASCII.GetString(decodedBytes)
