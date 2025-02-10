namespace TomsDataOnion.Library


module Layer1 =
    let Parse (payload: string) =
        let fiddleBits (b: byte) = System.Byte.RotateRight(b ^^^ 85uy, 1)
        let bytes = Layer0.Ascii85DecodeBytes payload
        let test = Array.map fiddleBits bytes
        System.Text.Encoding.ASCII.GetString(Array.map fiddleBits bytes)
