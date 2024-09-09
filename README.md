# WebSocket Chat Application

This project demonstrates a **real-time chat application** using **WebSocket** and **ASP.NET Core Web API**. Multiple clients can connect to the server and exchange messages in real time. Clients use a simple HTML page to connect to the WebSocket server.

## Features

- **WebSocket Support**: The server accepts WebSocket connections and relays messages between connected clients.
- **One-to-One Messaging**: Messages sent by one client are broadcasted to other clients in real time.
- **Static File Serving**: The server serves an HTML page, allowing clients to establish WebSocket connections and communicate.
- **Secure WebSocket Connection**: Uses `wss://` for secure WebSocket communication when the server is running over HTTPS.

## Technologies Used

- ASP.NET Core Web API
- WebSockets
- HTML & JavaScript

## Setup Instructions

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
- A modern web browser for running the HTML client (e.g., Chrome, Firefox).

### Clone the repository

```bash
git clone https://github.com/your-username/websocket-chat-app.git
cd websocket-chat-app
