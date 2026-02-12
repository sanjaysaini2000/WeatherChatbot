import React, { useEffect, useRef, useState } from 'react';
import './WeatherChat.css';

const API_BASE = process.env.REACT_APP_API_BASE || 'http://localhost:5000';

const QUICK_PROMPTS = [
  'What is the weather in New York?',
  'Weather in Tokyo',
  'Is it raining in London?',
  'Weather in Cape Town'
];

const formatNumber = (value) => {
  if (typeof value !== 'number' || Number.isNaN(value)) {
    return '';
  }

  return new Intl.NumberFormat('en-US', { maximumFractionDigits: 1 }).format(value);
};

function WeatherChat() {
  const [messages, setMessages] = useState([
    {
      id: 'welcome',
      role: 'assistant',
      text: 'Hi! Ask me about the weather in any city, region, or zip code.'
    }
  ]);
  const [input, setInput] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const listEndRef = useRef(null);

  useEffect(() => {
    listEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages, isLoading]);

  const sendMessage = async (text) => {
    const trimmed = text.trim();
    if (!trimmed || isLoading) {
      return;
    }

    const userMessage = {
      id: `user-${Date.now()}`,
      role: 'user',
      text: trimmed
    };

    setMessages((prev) => [...prev, userMessage]);
    setInput('');
    setIsLoading(true);

    try {
      const response = await fetch(`${API_BASE}/api/chat`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ message: trimmed })
      });

      const payload = await response.json().catch(() => null);
      if (!response.ok) {
        const errorMessage = payload?.error || 'Unable to fetch the weather right now.';
        throw new Error(errorMessage);
      }

      const assistantMessage = {
        id: `assistant-${Date.now()}`,
        role: 'assistant',
        text: payload.reply || 'Here is the latest weather update.',
        weather: payload
      };

      setMessages((prev) => [...prev, assistantMessage]);
    } catch (error) {
      const assistantMessage = {
        id: `assistant-${Date.now()}`,
        role: 'assistant',
        text: error.message || 'Something went wrong while contacting the weather service.',
        isError: true
      };
      setMessages((prev) => [...prev, assistantMessage]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    sendMessage(input);
  };

  return (
    <div className="weather-shell">
      <div className="weather-window">
        <div className="weather-titlebar">
          <div className="window-dots">
            <span />
            <span />
            <span />
          </div>
          <div>
            <h1>Weather Assistant</h1>
            <p>Real-time updates powered by WeatherAPI</p>
          </div>
        </div>

        <div className="weather-conversation">
          {messages.map((message) => (
            <div
              key={message.id}
              className={`message ${message.role} ${message.isError ? 'error' : ''}`}
            >
              <p>{message.text}</p>
              {message.weather && (
                <div className="weather-card">
                  <div className="weather-card-header">
                    {message.weather.iconUrl && (
                      <img src={message.weather.iconUrl} alt={message.weather.condition} />
                    )}
                    <div>
                      <h3>{message.weather.location}</h3>
                      <span>{message.weather.condition}</span>
                    </div>
                  </div>
                  <div className="weather-grid">
                    <div>
                      <span className="label">Temp</span>
                      <strong>
                        {formatNumber(message.weather.temperatureC)}°C / {formatNumber(message.weather.temperatureF)}°F
                      </strong>
                    </div>
                    <div>
                      <span className="label">Feels like</span>
                      <strong>
                        {formatNumber(message.weather.feelsLikeC)}°C / {formatNumber(message.weather.feelsLikeF)}°F
                      </strong>
                    </div>
                    <div>
                      <span className="label">Humidity</span>
                      <strong>{message.weather.humidity}%</strong>
                    </div>
                    <div>
                      <span className="label">Wind</span>
                      <strong>{formatNumber(message.weather.windKph)} kph</strong>
                    </div>
                    <div>
                      <span className="label">Local time</span>
                      <strong>{message.weather.localTime}</strong>
                    </div>
                    <div>
                      <span className="label">Observed</span>
                      <strong>{message.weather.observedAt}</strong>
                    </div>
                  </div>
                </div>
              )}
            </div>
          ))}

          {isLoading && (
            <div className="message assistant typing">
              <p>Checking the sky...</p>
            </div>
          )}
          <div ref={listEndRef} />
        </div>

        <div className="quick-prompts">
          {QUICK_PROMPTS.map((prompt) => (
            <button key={prompt} type="button" onClick={() => sendMessage(prompt)}>
              {prompt}
            </button>
          ))}
        </div>

        <form className="weather-input" onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Ask about the weather in any location..."
            value={input}
            onChange={(event) => setInput(event.target.value)}
          />
          <button type="submit" disabled={isLoading}>
            {isLoading ? 'Loading...' : 'Send'}
          </button>
        </form>
      </div>
    </div>
  );
}

export default WeatherChat;
