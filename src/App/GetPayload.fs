module GetPayload

let GetPayload (input: string) =
    let payloadIndex = input.IndexOf("==[ Payload ]")
    let startIndex = input.IndexOf("<~", payloadIndex)
    let endIndex = input.IndexOf("~>", startIndex)
    input.Substring(startIndex, endIndex - startIndex + 2)
