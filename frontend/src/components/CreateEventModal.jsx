// src/components/CreateEventModal.jsx
import React, { useState } from "react";
import { api } from "../api";
import "./CreateEventModal.css";

function CreateEventModal({ isOpen, onClose, onSuccess }) {
  const [newEvent, setNewEvent] = useState({
    title: "",
    description: "",
    date: "",
    basePoints: 100,
    difficulty: 1,
    category: "",
    prizeNames: []
  });
  const [prizeInput, setPrizeInput] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");
  
  const user = JSON.parse(localStorage.getItem("user"));

  if (!isOpen) return null;

  const addPrize = () => {
    if (prizeInput.trim()) {
      setNewEvent({
        ...newEvent,
        prizeNames: [...newEvent.prizeNames, prizeInput.trim()]
      });
      setPrizeInput("");
    }
  };

  const removePrize = (index) => {
    setNewEvent({
      ...newEvent,
      prizeNames: newEvent.prizeNames.filter((_, i) => i !== index)
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage("");

    try {
      const eventData = {
        title: newEvent.title,
        description: newEvent.description,
        date: newEvent.date,
        basePoints: newEvent.basePoints,
        difficulty: newEvent.difficulty,
        category: newEvent.category,
        prizeNames: newEvent.prizeNames
      };

      await api.post("/api/events", eventData, {
        headers: { "X-User-Id": user.Id || user.id }
      });

      setMessage({ type: "success", text: "Мероприятие создано!" });
      setTimeout(() => {
        onSuccess();
        onClose();
      }, 1000);
    } catch (err) {
      setMessage({ type: "error", text: err.response?.data || "Ошибка при создании" });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>Создать мероприятие</h2>
          <button className="modal-close" onClick={onClose}>×</button>
        </div>
        
        {message && <div className={`message ${message.type}`}>{message.text}</div>}
        
        <form onSubmit={handleSubmit}>
          <input 
            type="text" 
            placeholder="Название мероприятия *" 
            value={newEvent.title} 
            onChange={(e) => setNewEvent({...newEvent, title: e.target.value})} 
            required 
          />
          
          <textarea 
            rows="3" 
            placeholder="Описание" 
            value={newEvent.description} 
            onChange={(e) => setNewEvent({...newEvent, description: e.target.value})} 
          />
          
          <div className="form-row">
            <input 
              type="date" 
              value={newEvent.date} 
              onChange={(e) => setNewEvent({...newEvent, date: e.target.value})} 
              required 
            />
            <input 
              type="number" 
              placeholder="Баллы" 
              value={newEvent.basePoints} 
              onChange={(e) => setNewEvent({...newEvent, basePoints: parseInt(e.target.value)})} 
            />
            <input 
              type="number" 
              step="0.1" 
              placeholder="Сложность" 
              value={newEvent.difficulty} 
              onChange={(e) => setNewEvent({...newEvent, difficulty: parseFloat(e.target.value)})} 
            />
          </div>
          
          <select 
            value={newEvent.category} 
            onChange={(e) => setNewEvent({...newEvent, category: e.target.value})}
          >
            <option value="">Выберите категорию</option>
            <option value="IT">IT</option>
            <option value="Медиа">Медиа</option>
            <option value="Социальное проектирование">Социальное проектирование</option>
          </select>
          
          <div className="prize-input-group">
            <input 
              type="text" 
              value={prizeInput} 
              onChange={(e) => setPrizeInput(e.target.value)} 
              placeholder="Приз (мерч, билеты, стажировки)" 
            />
            <button type="button" onClick={addPrize}>Добавить</button>
          </div>
          
          <div className="prizes-list">
            {newEvent.prizeNames.map((p, i) => (
              <span key={i}>
                {p} 
                <button type="button" onClick={() => removePrize(i)}>×</button>
              </span>
            ))}
          </div>
          
          <button type="submit" className="btn-primary" disabled={loading}>
            {loading ? "Создание..." : "Создать мероприятие"}
          </button>
        </form>
      </div>
    </div>
  );
}

export default CreateEventModal;