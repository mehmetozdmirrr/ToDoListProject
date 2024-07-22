import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './Notes.css';  // CSS dosyasına doğru yolu belirleyin

const Notes = () => {
  const [notes, setNotes] = useState([]);
  const [title, setTitle] = useState('');
  const [text, setText] = useState('');
  const [editMode, setEditMode] = useState(false);
  const [editNoteId, setEditNoteId] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchNotes = async () => {
      const token = localStorage.getItem('token');
      const id = localStorage.getItem('id');
      if (!token) {
        window.location.href = '/';
        return;
      }

      try {
        const response = await axios.get(`https://localhost:7117/api/Note?UserId=${id}`, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        setNotes(response.data);
      } catch (error) {
        console.error(error);
        window.location.href = '/';
      }
    };

    fetchNotes();
  }, []);


  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('id');
    navigate('/');
  };

  const handleAddNote = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('token');
    const userId = localStorage.getItem('id');

    try {
      const response = await axios.post('https://localhost:7117/api/Note', {
        title,
        text,
        userId
      }, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      setNotes([...notes, response.data]);
      setTitle('');
      setText('');
    } catch (error) {
      console.error(error);
    }
  };

  const handleEditNote = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('token');
    const userId = localStorage.getItem('id');

    try {
      await axios.put(`https://localhost:7117/api/Note/${editNoteId}`, {
        title,
        text,
        userId
      }, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      const updatedNotes = notes.map(note =>
        note.id === editNoteId ? { ...note, title, text } : note
      );

      setNotes(updatedNotes);
      setTitle('');
      setText('');
      setEditMode(false);
      setEditNoteId(null);
    } catch (error) {
      console.error(error);
    }
  };
  

  const handleEditClick = (note) => {
    setTitle(note.title);
    setText(note.text);
    setEditMode(true);
    setEditNoteId(note.id);
  };

  const handleCancelEdit = () => {
    setTitle('');
    setText('');
    setEditMode(false);
    setEditNoteId(null);
  };

 const handleDeleteNote = async (id) => {
  debugger;
    const token = localStorage.getItem('token');
    //const userId = localStorage.getItem('id');

    try {
      await axios.delete(`https://localhost:7117/api/Note?id=` + id, {
        headers: {
          Authorization: `Bearer ${token}`
        },

      });

      setNotes(notes.filter(note => note.id !== id));
    } catch (error) {
      console.error("Error deleting note:", error);
    }
  };
  return (
    <div>
      <h1>Notlar</h1>
      <button onClick={handleLogout}>Çıkış Yap</button> {/* Çıkış Yap butonu */}
      <form onSubmit={editMode ? handleEditNote : handleAddNote}>
        <div>
          <label>Başlık:</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Not:</label>
          <textarea
            value={text}
            onChange={(e) => setText(e.target.value)}
            required
          />
        </div>
        <button type="submit">{editMode ? 'Notu Güncelle' : 'Not Ekle'}</button>
        {editMode && <button type="button" onClick={handleCancelEdit}>İptal</button>}
      </form>
      <ul>
        {notes.map(note => (
          <li key={note.id}>
            <h2>{note.title}</h2>
            <p>{note.text}</p>
            <button onClick={() => handleEditClick(note)}>Düzenle</button>
            <button onClick={() => handleDeleteNote(note.id)}>Sil</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Notes;
