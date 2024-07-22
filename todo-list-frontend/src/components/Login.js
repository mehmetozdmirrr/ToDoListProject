import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './Login.css';  // CSS dosyasına doğru yolu belirleyin


const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      debugger;
      const response = await axios.post('https://localhost:7117/api/User/login', {
        username,
        password
      });
      const token = response.data;
      localStorage.setItem("token", token.token);  // Token'ı localStorage'a kaydetme
      localStorage.setItem("id",token.userId)
      navigate('/notes');
    } catch (error) {
      console.error('Giriş yaparken bir hata oluştu!', error);
    }
  };
  return (
    <div>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Kullanıcı Adı:</label>
          <input 
            type="text" 
            value={username} 
            onChange={(e) => setUsername(e.target.value)} 
          />
        </div>
        <div>
          <label>Şifre:</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
          />
        </div>
        <button type="submit">Giriş Yap</button>
      </form>
      <button 
        onClick={() => navigate('/adduser')} 
        className="btn-secondary"
      >
        Kayıt Ol
      </button>
    </div>
  );
};

export default Login;