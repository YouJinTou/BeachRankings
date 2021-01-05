#load "Library.fs"
open BR.Core

let p = {Continent = "ABC"; WaterBody = "WTF"}
let s = Score.create [Score.BeachCleanliness 8.1]
let b = Beach.create (Beach.Name "Rusalka") p (Beach.Coordinates "123,456") s (System.Guid.NewGuid())  
