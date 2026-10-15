const { ethers } = require("hardhat");
const d = require("../deployment.json");
const last = require("../last-message.json");

async function main() {
  const receiver = await ethers.getContractAt("MessageReceiver", d.receiver);

  console.log("Receiver:", d.receiver);
  console.log("Messages received:", (await receiver.messagesReceived()).toString());
  console.log("lastMessageId:", await receiver.lastMessageId());
  console.log("lastSender (Messenger):", await receiver.lastSender());
  console.log("lastSourceChainSelector:", (await receiver.lastSourceChainSelector()).toString());
  console.log("lastText:", await receiver.lastText());

  const ok = (await receiver.lastMessageId()) === last.messageId;
  console.log(ok ? "\nDELIVERED: messageId matches the sent one." : "\nThe message has not been delivered yet; please try again in a few minutes.");
}

main().catch((e) => { console.error(e); process.exit(1); });