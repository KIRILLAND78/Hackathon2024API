import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';

function FileList({ onLogout }) {
    const [files, setFiles] = useState([]);
    const [selectedFiles, setSelectedFiles] = useState([]);
    const [uploadResults, setUploadResults] = useState(null);
    const [userEmail, setUserEmail] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const token = localStorage.getItem('accessToken');
        if (token) {
            const decodedToken = jwtDecode(token);
            setUserEmail(decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]);
        }

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

    const handleFileChange = (event) => {
        setSelectedFiles(event.target.files);
    };

    const handleUpload = () => {
        const formData = new FormData();
        for (const file of selectedFiles) {
            formData.append('files', file);
        }

        fetch('https://localhost:7248/File', {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
            },
            body: formData,
        })
        .then(response => response.json())
        .then(data => {
            setUploadResults(data);
            // Optionally, you can refresh the file list here by re-fetching files
        })
        .catch(error => console.error('Error uploading files:', error));
    };

    return (
        <div className="file-list-container">
            <div className="header">
                <h1>Ваши файлы</h1>
                <div className="user-info">
                    <span>{userEmail}</span>
                    <button onClick={onLogout}>Выйти</button>
                </div>
            </div>
            {loading ? (
                <p>Loading files...</p>
            ) : error ? (
                <p>Error: {error}</p>
            ) : (
                files.map(file => (
                    <div key={file.id} className="file-card">
                        <p>Название: {file.name}</p>
                        <p>Дата создания: {new Date(file.creationDate).toLocaleDateString()}</p>
                        <p>Объем файла: {file.size} MB</p>
                    </div>
                ))
            )}
            <div className="file-upload-container">
                <input type="file" multiple onChange={handleFileChange} />
                <button onClick={handleUpload}>Отправить</button>
            </div>
            {uploadResults && (
                <div className="upload-results">
                    <h2>Результаты загрузки</h2>
                    {Object.keys(uploadResults).map((fileName, index) => (
                        <p key={index}>{fileName}: {uploadResults[fileName]}</p>
                    ))}
                </div>
            )}
        </div>
    );
}

export default FileList;
