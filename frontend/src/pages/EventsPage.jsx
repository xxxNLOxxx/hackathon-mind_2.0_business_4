// src/pages/EventsPage.jsx
import React, { useEffect, useState } from "react";
import EventCard from "../components/EventCard";
import { api } from "../api";
import CreateEventModal from "../components/CreateEventModal";

function EventsPage() {
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showCreateModal, setShowCreateModal] = useState(false);
  
  const user = JSON.parse(localStorage.getItem("user"));
  const canCreateEvents = user?.role === "Organizer" || user?.role === "Admin";

  const fetchEvents = () => {
    setLoading(true);
    api.get("/events")
      .then(res => {
        setEvents(res.data);
        setLoading(false);
      })
      .catch(err => {
        console.error(err);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchEvents();
  }, []);

  const handleEventCreated = () => {
    fetchEvents();
  };

  if (loading) {
    return <div className="empty-state">Загрузка мероприятий...</div>;
  }

  return (
    <div className="fade-in">
      <div className="page-header">
        <h1>Мероприятия</h1>
        {canCreateEvents && (
          <button className="btn-primary" onClick={() => setShowCreateModal(true)}>
            + Создать мероприятие
          </button>
        )}
      </div>
      
      {events.length === 0 ? (
        <div className="empty-state">Пока нет мероприятий</div>
      ) : (
        <div className="grid-3">
          {events.map(event => (
            <EventCard key={event.id} event={event} />
          ))}
        </div>
      )}
      
      {canCreateEvents && (
        <CreateEventModal 
          isOpen={showCreateModal}
          onClose={() => setShowCreateModal(false)}
          onSuccess={handleEventCreated}
        />
      )}
    </div>
  );
}

export default EventsPage;