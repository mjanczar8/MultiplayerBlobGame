const playerId = window.location.pathname.split("/").pop();

const loadplayer = async () => {
    try {
            const response = await fetch(`/player/${playerId}`);
            if (!response.ok) throw new Error("Failed to fetch player details");
            const player = await response.json();
    
            document.getElementById("userName").value = player.userName;
            document.getElementById("gamesPlayed").value = player.gamesPlayed;
            document.getElementById("highestMass").value = player.highestMass;
            document.getElementById("kills").value = player.kills;


        } catch (err) {
                console.error("Error loading player:", err);
                alert("Failed to load player details");
        }
};
    
loadplayer();
    
// Handle form submission
document.getElementById("editForm").addEventListener("submit", async (event) => {
    event.preventDefault();
    
    const updatedPlayer = {
            userName: document.getElementById("userName").value,
            gamesPlayed: document.getElementById("gamesPlayed").value,
            highestMass: document.getElementById("highestMass").value,
            kills: document.getElementById("kills").value,
        };
    
    try {
            const response = await fetch(`/updateplayer/${playerId}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(updatedPlayer),
            });
    
            const result = await response.json();
    
            if (!response.ok) {
                console.error("Server response:", result);
                throw new Error(result.error || "Failed to update player");
            }
    
            alert("Player updated successfully!");
            window.location.href = "/";
        } catch (err) {
                console.error("Error updating player:", err);
                alert("Failed to update player.");
        }
});