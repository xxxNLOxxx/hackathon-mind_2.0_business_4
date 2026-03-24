// src/components/EventCard.jsx
import React from "react";
import "./EventCard.css";

function EventCard({ event }) {
  return (
    <div className="event-card">
      <h3 className="event-title">{event.title}</h3>
      <p className="event-desc">{event.description}</p>
      <div className="event-meta">
        <p>Баллы: {event.basePoints}</p>
        <p>Сложность: {event.difficulty}</p>
        <p>Категория: {event.category}</p>
        <p>Организатор: {event.organizerName}</p>
        <p>Дата: {new Date(event.date).toLocaleDateString()}</p>
        <p>Участников: {event.participantsCount}</p>
        <p>Призы: {event.prizes?.join(", ")}</p>
      </div>
    </div>
  );
}

export default EventCard;