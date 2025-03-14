const express = require("express")
const path = require("path")

const mongoose = require("mongoose")
const bodyParser = require("body-parser")

const { nanoid } = require("nanoid");

const app = express();
const port = 3000

const Player = require("./models/Players")

app.use(bodyParser.json());
app.use(express.urlencoded({extended:true}));
app.use(express.static(path.join(__dirname, "public")))

const mongoURI = "mongodb+srv://cjnova:word123@cluster0.wx9s6.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
mongoose.connect(mongoURI);

const db = mongoose.connection;

db.on("error", console.error.bind(console, "MongoDB connection error"));
db.once("open", ()=>{
    console.log("Connected to MongoDB Database");
});

app.get("/player", async (req, res)=>{
    try{
        const players = await Player.find().sort({ highestMass: -1 });;
        res.json(players);
        console.log(players);
    }catch(err){
        console.log("Failed to get players.");
    }
});


app.get("/", (req,res)=>{
    res.sendFile(path.join(__dirname, "public", "html/mainlist.html"))
})
app.get("/addtolist", (req,res)=>{
    res.sendFile(path.join(__dirname,"public", "html/addtoList.html"))
})
app.get("/edit/:id", (req, res) => {
    res.sendFile(path.join(__dirname, "public", "html", "edit.html"));
});

app.post("/addtolist", async (req,res)=>{
    try{
        const newPlayer = new Player(req.body)
        const savePlayer = await newPlayer.save()
        res.redirect("/")
        console.log(savePlayer)
    }catch(err){
        res.status(501).json({error:"Failed to add new Player."});
    }
})

app.put("/updateplayer/:id", async (req, res) => {
    try {
        const updatedplayer = await Player.findByIdAndUpdate(
            req.params.id, 
            { userName:req.body.userName, gamesPlayed:req.body.gamesPlayed, highestMass:req.body.highestMass, kills:req.body.kills }, 
            { new: true } 
        );

        if (!updatedplayer) {
            return res.status(404).json({ error: "player not found" });
        }

        res.json(updatedplayer);
    } catch (err) {
        console.error(err);
        res.status(500).json({ error: "Failed to update player" });
    }
});

app.get("/player/:id", async (req, res) => {
    try {
        const player = await Player.findById(req.params.id);

        if (!player) {
            return res.status(404).json({ error: "Player not found" });
        }

        res.json(player);
    } catch (error) {
        console.error("Error retrieving player:", error);
        res.status(500).json({ error: "Failed to retrieve player" });
    }
});

app.post("/sentdata", (req,res)=>{
    const newPlayerData = req.body;

    console.log(JSON.stringify(newPlayerData,null,2));

    res.json({message:"Player Data recieved"});
});

app.delete("/delete/:id", async (req, res) => {
    try {
        const player = await Player.findById(req.params.id);

        if (!player) {
            return res.status(404).json({ error: "Player not found" });
        }

        await Player.findByIdAndDelete(req.params.id);
        res.json({ message: "Player deleted successfully" });

    } catch (err) {
        console.error(err);
        res.status(500).json({ error: "Failed to delete player" });
    }
});

app.post("/sentdatatodb", async (req,res)=>{
    try{
        const newPlayerData = req.body;

        console.log(JSON.stringify(newPlayerData,null,2));

        const newPlayer = new Player({
            playerid:nanoid(8),
            screenName:newPlayerData.screenName,
            firstName:newPlayerData.firstName,
            lastName:newPlayerData.lastName,
            dateStartedPlaying:newPlayerData.dateStartedPlaying,
            score:newPlayerData.score

        });
        //save to database
        await newPlayer.save();
        res.json({message:"Player Added Successfully",playerid:newPlayer.playerid, name:newPlayer.screenNameName});
    }
    catch(error){
        res.status(500).json({error:"Failed to add player"})
    }
    
    
});

//Update Player
app.put("/updateplayer/:id", async (req, res) => {
    try {
        const updatedPlayer = await Player.findByIdAndUpdate(
            req.params.id,
            {
                userName: req.body.userName,
                gamesPlayed: parseInt(req.body.gamesPlayed),
                highestMass: parseInt(req.body.highestMass),
                kills: parseInt(req.body.kills),
            },
            { new: true }
        );

        if (!updatedPlayer) {
            return res.status(404).json({ error: "Player not found" });
        }

        res.json(updatedPlayer);
    } catch (err) {
        console.error("Error updating player:", err);
        res.status(500).json({ error: "Failed to update player" });
    }
});


app.listen(3000, ()=>{
    console.log("Running on port 3000");
})