const mongoose = require("mongoose");

const playerSchema = new mongoose.Schema({
    userName:String,
    gamesPlayed:Number,
    highestMass:Number,
    kills:Number
})

module.exports = mongoose.model("Player", playerSchema);