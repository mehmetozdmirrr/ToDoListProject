import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './components/Login';
import Notes from './components/Notes';
import AddUser from './components/AddUser';



const App = () => {
  const [token, setToken] = useState('');

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login setToken={setToken} />} />
        <Route path="/notes" element={<Notes token={token} />} />
        <Route path="/adduser" element={<AddUser />} />
      </Routes>
    </Router>
  );
};

export default App;   