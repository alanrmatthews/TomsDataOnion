namespace TomsDataOnion.Tests

module Tests =

    open System
    open Xunit
    open TomsDataOnion.Library

    [<Fact>]
    let ``TestIPV4Checksum`` () =
        let bytes =
            [| 0x45uy
               0x00uy
               0x00uy
               0x73uy
               0x00uy
               0x00uy
               0x40uy
               0x00uy
               0x40uy
               0x11uy
               0xb8uy
               0x61uy
               0xc0uy
               0xa8uy
               0x00uy
               0x01uy
               0xc0uy
               0xa8uy
               0x00uy
               0xc7uy |]

        Assert.True(Layer4.ipv4ChecksumCorrect (bytes, 0))
