namespace TomsDataOnion.Library


module Layer2 =
    let Peel (payload: string) =
        let bytes = Layer0.Ascii85DecodeBytes payload

        let IsParityCorrect (b: byte) =
            let bitsSet = System.Byte.PopCount(b >>> 1)

            let parity = b &&& 1uy
            if bitsSet % 2uy = 0uy then parity = 0uy else parity = 1uy

        let packFirst7Bits (input: byte[]) : byte[] =
            let seq = seq { for i in 0..8 .. (input.Length - 1) -> i }

            let packed =
                Seq.map
                    (fun i ->
                        [| input[i + 0] <<< 0 ||| (input[i + 1] >>> 7)
                           input[i + 1] <<< 1 ||| (input[i + 2] >>> 6)
                           input[i + 2] <<< 2 ||| (input[i + 3] >>> 5)
                           input[i + 3] <<< 3 ||| (input[i + 4] >>> 4)
                           input[i + 4] <<< 4 ||| (input[i + 5] >>> 3)
                           input[i + 5] <<< 5 ||| (input[i + 6] >>> 2)
                           input[i + 6] <<< 6 ||| (input[i + 7] >>> 1) |])
                    seq

            packed |> Seq.collect id |> Seq.toArray


        let byteArray =
            bytes
            |> Array.filter IsParityCorrect
            |> Array.map (fun f -> f &&& 254uy)
            |> packFirst7Bits

        System.Text.Encoding.ASCII.GetString(byteArray)
