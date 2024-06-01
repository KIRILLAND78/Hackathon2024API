import React, { useState, useEffect } from 'react';

function FileList({ user, onLogout }) {
  const [files, setFiles] = useState([]);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchFiles = async () => {
      try {
        const response = await fetch(`https://localhost:7248/api/File/MyFiles?Count=10&Page=0`, {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
            'Content-Type': 'application/json'
          }
        });

        if (!response.ok) {
          throw new Error('Failed to fetch files');
        }

        const result = await response.json();
        setFiles(result);
        setLoading(false);
      } catch (error) {
        setError(error.message);
        setLoading(false);
      }
    };

    fetchFiles();
  }, []);

  const handleLogout = () => {
    onLogout();
  };

  return (
    <div className="file-list-container">
      <div className="header">
        <h2>Your Files</h2>
        <div className="user-info">
          <span>{user.email}</span>
          <button onClick={handleLogout}>Logout</button>
        </div>
      </div>
      {loading ? (
        <p>Loading files...</p>
      ) : error ? (
        <p className="error-message">{error}</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Disk Location (Hash)</th>
              <th>Encrypted</th>
              <th>Owner ID</th>
            </tr>
          </thead>
          <tbody>
            {files.map((file) => (
              <tr key={file.id}>
                <td>{file.name}</td>
                <td>{file.diskLocation}</td>
                <td>{file.encrypted ? 'Yes' : 'No'}</td>
                <td>{file.ownerId}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default FileList;
