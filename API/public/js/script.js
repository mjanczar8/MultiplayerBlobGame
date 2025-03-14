const playerContainer = document.getElementById("player-container");

const fetchPlayers = async ()=>{
    try{
        //fetch data from server
        const response = await fetch("/player");
        if(!response.ok){
            throw new Error("Failed to get players");
        }

        //Parse json
        const player = await response.json();
        console.log(player);
        //Format the data to html
        playerContainer.innerHTML = "";

        player.forEach((player) => {
            const playerDiv = document.createElement("div");
            playerDiv.className = "player";
            playerDiv.innerHTML = `
            <li><b style="color: black;">Username:</b> ${player.userName}  <b style="color: black;">Games Played:</b> ${player.gamesPlayed}  <b style="color: black;">Highest Mass:</b> ${player.highestMass}  <b style="color: black;">Kills:</b> ${player.kills}</li> 
            <button onclick="Update('${player._id}')">Update</button>
            <button onclick="Delete('${player._id}')">Delete</button>
            `;

            

            playerContainer.appendChild(playerDiv);
        });
    }catch(error){
        console.error("Error: ", error);
        playerContainer.innerHTML = "<p style='color:red'>Failed to get users</p>";
    }
    
    
}

const Delete = async (id) => {
    if (!confirm("Are you sure you want to delete this Player?")) return;

    try {
        const response = await fetch(`/delete/${id}`, { method: "DELETE" });

        if (!response.ok) {
            throw new Error("Failed to delete player");
        }

        fetchPlayers();
    } catch (err) {
        console.error("Error deleting player:", err);
    }
};

const Update = async (id) => {
    window.location.href = `/edit/${id}`;
};


fetchPlayers();