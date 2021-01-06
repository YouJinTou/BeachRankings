open BR.Core.Beach
open BR.Core.Score

[<EntryPoint>]
let main argv =
    let n = Name "Rusalka"
    let p = { Continent = "Europe"; WaterBody = "Black Sea" }
    let c = Coordinates "123,456"
    let s = [SandQuality 8.; BeachCleanliness 4.] |> BR.Core.Score.create
    let b = BR.Core.Beach.create n p c s (System.Guid.NewGuid())
    printfn "%O" b
    0
