import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

function Register({ onRegister }) {
  const [email, setEmail] = useState('');
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      const response = await fetch('https://localhost:7248/api/Auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });

      const result = await response.json();
      if (!result.isSuccess) {
        throw new Error(result.errorMessage || 'Login failed');
      }

      const resultData = result.data;
      localStorage.setItem('accessToken', resultData.accessToken);
      localStorage.setItem('refreshToken', resultData.refreshToken);

      const decodedToken = jwtDecode(resultData.accessToken);
      const userData = {
        email: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
        isAdmin: decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'Admin'
      };

      onRegister(userData);
      navigate('/files');
    } catch (error) {
      setError(error.message);
      setTimeout(() => setError(''), 3000);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setError('Passwords do not match');
      return;
    }

    try {
      const response = await fetch('https://localhost:7248/api/Auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, login, password }),
      });

      const result = await response.json();
      if (!result.isSuccess) {
        throw new Error(result.errorMessage || 'Registration failed');
      }

      // Если регистрация успешна, выполнить логин
      await handleLogin();
    } catch (error) {
      setError(error.message);
      setTimeout(() => setError(''), 3000);
    }
  };

  return (
    <div className="register-container">
      <h2>Register</h2>
      <form onSubmit={handleSubmit} className={error ? 'error' : ''}>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
          required
        />
        <input
          type="text"
          value={login}
          onChange={(e) => setLogin(e.target.value)}
          placeholder="Login"
          required
        />
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Password"
          required
        />
        <input
          type="password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          placeholder="Confirm Password"
          required
        />
        {error && <p className="error-message">{error}</p>}
        <button type="submit">Register</button>
      </form>
      <button onClick={() => navigate('/login')}>Back to Login</button>
    </div>
  );
}

export default Register;
