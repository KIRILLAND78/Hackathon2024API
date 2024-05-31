import React, { useState } from 'react';
import FileActions from './FileActions';

function FileList({ user, onLogout }) {
  const [files, setFiles] = useState([
    { id: 1, name: 'file1.txt', createdAt: '2024-01-01', size: '15KB' },
    { id: 2, name: 'file2.txt', createdAt: '2024-01-02', size: '20KB' },
  ]);

  return (
    <div className="file-list-container">
      <header>
        <h1>Ваши файлы</h1>
        <div className="user-info">
          <span>{user.email}</span>
          <button onClick={onLogout}>Выйти</button>
        </div>
      </header>
      <FileActions files={files} setFiles={setFiles} />
    </div>
  );
}

export default FileList;
