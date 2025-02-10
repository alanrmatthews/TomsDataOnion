namespace TomsDataOnion.Library

module Layer0 =
    let Ascii85DecodeBytes (input: string) : byte[] =
        let GetBuffer (rawInput: string) =
            let input =
                rawInput.Substring(2, rawInput.Length - 4).Replace("\r", "").Replace("\n", "")

            let padding = 5 - (input.Length % 5)
            input.PadRight(input.Length + padding, 'u'), padding

        let convertMask (mask: int) : byte[] =
            let bytes = System.BitConverter.GetBytes(mask)

            if System.BitConverter.IsLittleEndian then
                System.Array.Reverse(bytes)

            bytes

        let buffer, padding = GetBuffer(input)
        let seq = seq { for i in 0..5 .. (buffer.Length - 1) -> i }

        let converted =
            Seq.map
                (fun i ->
                    let mask =
                        (int buffer.[i + 0] - 33) * 85 * 85 * 85 * 85
                        + (int buffer.[i + 1] - 33) * 85 * 85 * 85
                        + (int buffer.[i + 2] - 33) * 85 * 85
                        + (int buffer.[i + 3] - 33) * 85
                        + (int buffer.[i + 4] - 33)

                    convertMask mask)
                seq

        let result = converted |> Seq.collect id |> Seq.toArray
        result.[.. (result.Length - padding - 1)]

    let Ascii85Decode (input: string) : string =
        System.Text.Encoding.ASCII.GetString(Ascii85DecodeBytes input)
