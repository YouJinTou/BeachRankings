open BR.Core.Beach

[<EntryPoint>]
let main argv =
    let n = Name "Rusalka"
    let p = { Continent = "Europe"; WaterBody = "Black Sea" }
    let c = Coordinates "123,456"
    let s = createScore [SandQuality 8.; BeachCleanliness 4.]
    let b = create n p c s (System.Guid.NewGuid())
    printfn "%O" b
    0
