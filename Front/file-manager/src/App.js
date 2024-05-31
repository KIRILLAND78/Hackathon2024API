import React, { useState } from 'react';
import { Route, Routes, Navigate, useNavigate } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import FileList from './components/FileList';
import AdminPanel from './components/AdminPanel';

function App() {
  const [user, setUser] = useState(null);
  const [isAdmin, setIsAdmin] = useState(false);
  const navigate = useNavigate();

  const handleLogin = (userData) => {
    setUser(userData);
    if (userData.isAdmin) {
      setIsAdmin(true);
      navigate('/admin');
    } else {
      setIsAdmin(false);
      navigate('/files');
    }
  };

  const handleLogout = () => {
    setUser(null);
    setIsAdmin(false);
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    navigate('/login');
  };

  const handleRegister = (userData) => {
    setUser(userData);
    if (userData.isAdmin) {
      setIsAdmin(true);
      navigate('/admin');
    } else {
      setIsAdmin(false);
      navigate('/files');
    }
  };

  return (
    <div className="App">
      <Routes>
        <Route path="/login" element={<Login onLogin={handleLogin} />} />
        <Route path="/register" element={<Register onRegister={handleRegister} />} />
        <Route path="/files" element={user ? <FileList user={user} onLogout={handleLogout} /> : <Navigate to="/login" />} />
        <Route path="/admin" element={isAdmin ? <AdminPanel onLogout={handleLogout} /> : <Navigate to="/login" />} />
        <Route path="/" element={<Navigate to="/login" />} />
      </Routes>
    </div>
  );
}

export default App;
