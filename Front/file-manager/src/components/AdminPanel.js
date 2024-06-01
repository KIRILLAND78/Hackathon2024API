import React, { useState } from 'react';

function AdminPanel({ onLogout }) {
    const [users, setUsers] = useState([
        { UserId: 1, maxFilesCount: 5, canUpload: true, canRead: true, imageQuality: 80, canChange: true, canDelete: true, maxFileSizeMb: 35 },
        { userId: 2, email: 'user2@example.com', maxFilesCount: 10, canUpload: true, canRead: true, imageQuality: 90, canChange: false, canDelete: true, maxFileSizeMb: 50 },
        { userId: 3, email: 'admin@example.com', maxFilesCount: 20, canUpload: true, canRead: true, imageQuality: 100, canChange: true, canDelete: true, maxFileSizeMb: 100 },
    ]);

    const handleChange = (index, key, value) => {
        const updatedUsers = [...users];
        updatedUsers[index][key] = value;
        setUsers(updatedUsers);
    };

    const handleSave = (user) => {
        console.log('Sending user data to server:', user);
        fetch('http://localhost:5181/api/Admin', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                UserId: user.userId,
                MaxFilesCount: user.maxFilesCount,
                CanUpload: user.canUpload,
                canRead: user.canRead,
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


    return (
        <div className="admin-panel-container">
            <div>
                <h1>User Details</h1>
                {users.map((user, index) => (
                    <div key={index}>
                        <h2>User ID: {user.userId}</h2>
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
