using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class ChessAI : MonoBehaviour
{
    private string apiUrl = "https://lichess.org/api/cloud-eval"; // Lichess API for Stockfish
    private string gameStateFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; // Example FEN notation (starting position)

    // Start is called before the first frame update
    void Start()
    {
        // Fetch the best move for AI after a delay to simulate game setup
        StartCoroutine(GetBestMove());
    }

    // Coroutine to get the best move from Lichess API
    private IEnumerator GetBestMove()
    {
        // Set up the request URL with the current FEN
        string url = apiUrl + "?fen=" + gameStateFEN + "&multiPv=1";  // multiPv=1 to get the top move

        // Send GET request to the API
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        // Check if the request is successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response using JSONUtility
            string response = request.downloadHandler.text;
            Debug.Log("AI Response: " + response);

            // Deserialize the response into a C# object
            var moveData = JsonUtility.FromJson<JsonResponse>(response);

            if (moveData != null && moveData.pvs.Length > 0)
            {
                // Get the best move from the highest evaluated sequence
                string bestMove = moveData.pvs[0].moves; // Best move sequence
                Debug.Log("Best Move Sequence: " + bestMove);

                // You can now use `bestMove` to make the AI's move on the game board
                SaveMoveToFile(bestMove);
            }
            else
            {
                Debug.LogError("No moves found in the response.");
            }
        }
        else
        {
            Debug.LogError("API Request Failed: " + request.error);
        }
    }

    // Define classes to match the structure of the JSON response
    [System.Serializable]
    public class JsonResponse
    {
        public PV[] pvs; // Array of possible move sequences
    }

    [System.Serializable]
    public class PV
    {
        public string moves; // The sequence of moves in UCI format
        public int cp; // Evaluation score in centipawns
    }

    // Class to hold the data to save to the JSON file
    [System.Serializable]
    public class MoveData
    {
        public string fen; // The current FEN of the game
        public string bestMove; // The best move (in UCI format)
    }

    // Save the best move and FEN to a JSON file
    private void SaveMoveToFile(string bestMove)
    {
        // Create a structure to hold the best move data
        MoveData moveData = new MoveData
        {
            fen = gameStateFEN,
            bestMove = bestMove
        };

        // Serialize the data to JSON
        string json = JsonUtility.ToJson(moveData);

        // Define the file path to save the JSON data
        string filePath = Path.Combine(Application.persistentDataPath, "best_move.json");

        // Write the JSON data to a file
        File.WriteAllText(filePath, json);

        Debug.Log("Best move saved to: " + filePath);
    }
}
