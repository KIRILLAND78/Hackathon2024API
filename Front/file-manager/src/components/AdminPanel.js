import React, { useState, useEffect } from 'react';

function AdminPanel({ onLogout }) {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetch('https://localhost:7248/api/Admin/Index?Count=10&Page=0', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            setUsers(data);
            setLoading(false);
        })
        .catch(error => {
            setError(error);
            setLoading(false);
        });
    }, []);

    const handleChange = (index, key, value) => {
        const updatedUsers = [...users];
        updatedUsers[index][key] = value;
        setUsers(updatedUsers);
    };

    const handleSave = (user) => {
        console.log('Sending user data to server:', user);
        fetch('https://localhost:7248/api/Admin', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            },
            body: JSON.stringify({
                UserId: user.id,
                MaxFilesCount: user.maxFilesCount,
                CanUpload: user.canUpload,
                CanRead: user.canRead,
                ImageQuality: user.imageQuality,
                CanChange: user.canChange,
                CanDelete: user.canDelete,
                MaxFileSizeMb: user.maxFileSizeMb,
            }),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('User saved:', data);
            // Дополнительные действия при успешном сохранении
        })
        .catch(error => {
            console.error('There was an error saving the user!', error);
        });
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error.message}</div>;
    }

    return (
        <div className="admin-panel-container">
            <div>
                <h1>User Details</h1>
                {users.map((user, index) => (
                    <div key={user.id} className="user-card">
                        <h2>User ID: {user.id}</h2>
                        <p>Email: {user.email}</p>
                        <p>Max Files Count: <input type="number" value={user.maxFilesCount} onChange={e => handleChange(index, 'maxFilesCount', e.target.value)} /></p>
                        <p>Can Upload: <input type="checkbox" checked={user.canUpload} onChange={e => handleChange(index, 'canUpload', e.target.checked)} /></p>
                        <p>Can Read: <input type="checkbox" checked={user.canRead} onChange={e => handleChange(index, 'canRead', e.target.checked)} /></p>
                        <p>Image Quality: <input type="number" value={user.imageQuality} onChange={e => handleChange(index, 'imageQuality', e.target.value)} /></p>
                        <p>Can Change: <input type="checkbox" checked={user.canChange} onChange={e => handleChange(index, 'canChange', e.target.checked)} /></p>
                        <p>Can Delete: <input type="checkbox" checked={user.canDelete} onChange={e => handleChange(index, 'canDelete', e.target.checked)} /></p>
                        <p>Max File Size (MB): <input type="number" value={user.maxFileSizeMb} onChange={e => handleChange(index, 'maxFileSizeMb', e.target.value)} /></p>
                        <button onClick={() => handleSave(user)}>Save</button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default AdminPanel;
