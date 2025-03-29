import React, { useEffect, useState } from 'react';
import { Table, Button, Container, Alert, Toast, ToastContainer } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
    const [workflows, setWorkflows] = useState([]);
    const [error, setError] = useState(null);
    const [toast, setToast] = useState({ show: false, message: '', variant: 'success' });

    const fetchData = () => {
        fetch('/workflows/all')
            .then((res) => res.json())
            .then((result) => {
                if (result.success) {
                    setWorkflows(result.data);
                } else {
                    setError(result.error.message || 'Failed to load data');
                }
            })
            .catch((err) => {
                setError(err.message);
            });
    };

    useEffect(() => {
        fetchData();
    }, []);

    const handleRunWorkflow = (workflowId) => {
        fetch(`/workflows/${workflowId}/run`, { method: 'POST' })
            .then((res) => res.json())
            .then((response) => {
                if (response.success) {
                    setToast({ show: true, message: `Workflow ${workflowId} started successfully.`, variant: 'success' });
                } else {
                    setToast({ show: true, message: response.error.message, variant: 'danger' });
                }
            })
            .catch((err) => {
                setToast({ show: true, message: `Error: ${err.message}`, variant: 'danger' });
            });
    };

    return (
        <Container className="mt-5">
            <h3 className="mb-4">UniLoader Workflows</h3>
            {!error && (
                <Table bordered hover>
                    <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Active</th>
                        <th>Multi Exec Behavior</th>
                        <th>Run</th>
                    </tr>
                    </thead>
                    <tbody>
                    {workflows.map((wf) => (
                        <tr key={wf.id}>
                            <td>{wf.id}</td>
                            <td>{wf.name}</td>
                            <td>{wf.isActive ? 'Yes' : 'No'}</td>
                            <td>{wf.multiExecBehavior}</td>
                            <td>
                                <Button variant="primary" onClick={() => handleRunWorkflow(wf.id)}>
                                    Run
                                </Button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </Table>
            )}

            <ToastContainer position="top-end" className="p-3">
                <Toast onClose={() => setToast({ ...toast, show: false })} show={toast.show} delay={6000} autohide bg={toast.variant}>
                    <Toast.Header>
                        <strong className="me-auto">Notification</strong>
                    </Toast.Header>
                    <Toast.Body className="text-white">{toast.message}</Toast.Body>
                </Toast>
            </ToastContainer>
        </Container>
    );
}

export default App;