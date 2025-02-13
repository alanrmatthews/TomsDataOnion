module TomsDataOnion.App

open System.IO
open System.Reflection

open TomsDataOnion.Library
open GetPayload

let readEmbeddedResource resourceName =
    let assembly = Assembly.GetExecutingAssembly()
    use stream = assembly.GetManifestResourceStream(resourceName)
    use reader = new StreamReader(stream)
    reader.ReadToEnd()

let payload0 = GetPayload(readEmbeddedResource "App.input.dataonion.txt")

let layer1 = Layer0.Ascii85Decode payload0
let layer2 = Layer1.Peel(GetPayload layer1)
let layer3 = Layer2.Peel(GetPayload layer2)
let layer4 = Layer3.Peel(GetPayload layer3)
let layer5 = Layer4.Peel(GetPayload layer4)
printfn "%s" layer5
