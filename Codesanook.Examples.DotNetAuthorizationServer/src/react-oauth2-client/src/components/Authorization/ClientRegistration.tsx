import { ChangeEvent, FormEvent, useState } from 'react';
import axios from 'axios';

export default function ClientRegistration() {
  const [name, setName] = useState('');
  const [response, setResponse] = useState('');

  const handleFormSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const model = {
      name,
    };
    try {
      const response = await axios.post('/client/register', model);
      alert('client registered');
      setResponse(JSON.stringify(response.data, null, 2));
    } catch (error) {
      console.error(error);
    }
  };

  const handleNameChange = (event: ChangeEvent<HTMLInputElement>) => {
    setName(event.target.name);
  };

  return (
    <div>
      <form onSubmit={handleFormSubmit}>
        <p>
          Name:
          <input type='text' value={name} onChange={handleNameChange} />
        </p>
        <p>
          <input type='submit' value='register' />
        </p>
      </form>
      <pre>
        <code>{response}</code>
      </pre>
    </div>
  );
};

