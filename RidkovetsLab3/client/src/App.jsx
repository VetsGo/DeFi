import { useState } from 'react';
import { ethers } from 'ethers';

function App() {
  const [account, setAccount] = useState(null);
  const [history, setHistory] = useState([]);

  const connectWallet = async () => {
    if (window.ethereum) {
      try {
        const provider = new ethers.BrowserProvider(window.ethereum);
        const accounts = await provider.send("eth_requestAccounts", []);
        const userAddress = accounts[0];
        setAccount(userAddress);
        fetchHistory(userAddress);
      } catch (err) {
        console.error("Connection error:", err);
      }
    } else {
      alert("Install MetaMask!");
    }
  };

  const fetchHistory = async (walletAddress) => {
    try {
      const response = await fetch(`http://localhost:5000/api/swaps?trader=${walletAddress}`);
      const data = await response.json();
      setHistory(data);
    } catch (error) {
      Console.error("Error loading history:", error);
    }
  };

  return (
      <div style={{ padding: '20px', fontFamily: 'Arial' }}>
        <h1>RidkovetsLab3 - DeFi Pool</h1>
        {!account ? (
            <button onClick={connectWallet} style={{ padding: '10px 20px' }}>Connect MetaMask</button>
        ) : (
            <p><strong>Connected:</strong> {account}</p>
        )}

        <h2>Exchange History</h2>
        <table border="1" cellPadding="8" style={{ borderCollapse: 'collapse', width: '100%' }}>
          <thead>
          <tr>
            <th>Tx Hash</th>
            <th>Amount In</th>
            <th>Amount Out</th>
          </tr>
          </thead>
          <tbody>
          {history.length > 0 ? (
              history.map((item, index) => (
                  <tr key={index}>
                    <td>{item.transactionHash}</td>
                    <td>{item.amountIn}</td>
                    <td>{item.amountOut}</td>
                  </tr>
              ))
          ) : (
              <tr><td colSpan="3">No records found</td></tr>
          )}
          </tbody>
        </table>
      </div>
  );
}

export default App;