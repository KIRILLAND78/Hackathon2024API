import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

function Login({ onLogin }) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
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

      onLogin(userData);
    } catch (error) {
      setError(true);
      setTimeout(() => setError(false), 1000);
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <form onSubmit={handleSubmit} className={error ? 'error' : ''}>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
          required
        />
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Password"
          required
        />
        <button type="submit">Login</button>
      </form>
      <button onClick={() => navigate('/register')}>Register</button>
    </div>
  );
}

export default Login;
