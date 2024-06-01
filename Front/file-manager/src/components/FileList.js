import React, { useState, useEffect } from 'react';

function FileList({ user, onLogout }) {
  const [files, setFiles] = useState([]);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchFiles = async () => {
      try {
        const response = await fetch(`https://localhost:7248/File/MyFiles?Count=5&Page=0`, {
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
        <div className="files-grid">
          {files.map((file) => (
            <div key={file.id} className="file-card">
              <p><strong>Name:</strong> {file.name}</p>
              <p><strong>Disk Location (Hash):</strong> {file.diskLocation}</p>
              <p><strong>Encrypted:</strong> {file.encrypted ? 'Yes' : 'No'}</p>
              <p><strong>Owner ID:</strong> {file.ownerId}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default FileList;
