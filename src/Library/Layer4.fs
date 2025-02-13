namespace TomsDataOnion.Library


module Layer4 =
    let validateOnesComplementChecksum (bytes: byte[], checksumByte: int32) =
        let checksum =
            uint16 bytes[checksumByte + 0] <<< 8 ||| uint16 bytes[checksumByte + 1]

        let seq = seq { for i in 0..2 .. (bytes.Length - 1) -> i }

        let sum =
            let totalSum =
                Seq.fold (fun acc i -> acc + uint32 (uint16 bytes[i + 0] <<< 8 ||| uint16 bytes[i + 1])) 0u seq

            totalSum - uint32 checksum

        let carryAddition = uint16 ((sum &&& 0xFFFFu) + (sum >>> 16))
        ~~~carryAddition = checksum

    let ipv4ChecksumCorrect (packet: byte[]) =
        validateOnesComplementChecksum (packet.[0..19], 10)

    let udpChecksumCorrect (packet: byte[]) =
        let pseudoHeader =
            let header =
                Array.concat [ packet.[12..19]; [| 0uy; 17uy; packet[24]; packet[25] |]; packet.[20..] ]

            if packet.Length % 2 = 0 then
                header
            else
                Array.append header [| 0uy |]

        validateOnesComplementChecksum (pseudoHeader, 18)

    let Peel (payload: string) =
        let GetPacket (bytes: byte[]) =
            let mutable i = 0

            seq {
                while i < bytes.Length do
                    let length = int32 (uint16 bytes[i + 2] <<< 8 ||| uint16 bytes[i + 3])
                    let packet = bytes[i .. i + length - 1]
                    i <- i + length
                    yield packet
            }

        let GetPayload (packet: byte[]) = packet[28..]

        let bytes = Layer0.Ascii85DecodeBytes payload

        let sourceIPValid (packet: byte[]) =
            packet[12] = 10uy && packet[13] = 1uy && packet[14] = 1uy && packet[15] = 10uy

        let destinationIPValid (packet: byte[]) =
            packet[16] = 10uy && packet[17] = 1uy && packet[18] = 1uy && packet[19] = 200uy

        let IsPacketValid (packet: byte[]) =
            sourceIPValid packet
            && destinationIPValid packet
            && ipv4ChecksumCorrect packet
            && udpChecksumCorrect packet

        let byteArray =
            GetPacket bytes
            |> Seq.filter IsPacketValid
            |> Seq.map GetPayload
            |> Seq.collect id
            |> Seq.toArray

        System.Text.Encoding.ASCII.GetString(byteArray)