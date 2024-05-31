import React from 'react';

function FileActions({ files, setFiles }) {
  const handleAddFile = () => {
    const newFile = { id: files.length + 1, name: `file${files.length + 1}.txt`, createdAt: '2024-01-03', size: '25KB' };
    setFiles([...files, newFile]);
  };

  const handleDeleteFile = (id) => {
    setFiles(files.filter(file => file.id !== id));
  };

  const handleEditFile = (id) => {
    const newName = prompt("Enter new file name");
    setFiles(files.map(file => file.id === id ? { ...file, name: newName } : file));
  };

  return (
    <div className="file-actions">
      <button onClick={handleAddFile}>Добавить новый файл</button>
      <ul>
        {files.map(file => (
          <li key={file.id}>
            <span>{file.name}</span>
            <span>{file.createdAt}</span>
            <span>{file.size}</span>
            <button onClick={() => handleEditFile(file.id)}>Редактировать</button>
            <button onClick={() => handleDeleteFile(file.id)}>Удалить</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default FileActions;
