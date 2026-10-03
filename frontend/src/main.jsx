import React, { useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import './styles.css';

const API = import.meta.env.VITE_API_URL || 'http://localhost:5080';
function App() {
  const [songs, setSongs] = useState([]), [plans, setPlans] = useState([]), [query, setQuery] = useState(''), [message, setMessage] = useState('');
  useEffect(() => { Promise.all([fetch(`${API}/music`).then(r => r.json()), fetch(`${API}/plans`).then(r => r.json())]).then(([music, availablePlans]) => { setSongs(music); setPlans(availablePlans); }).catch(() => setMessage('No se pudo conectar con la API.')); }, []);
  const search = async event => { event.preventDefault(); const response = await fetch(`${API}/music?search=${encodeURIComponent(query)}`); setSongs(await response.json()); };
  const subscribe = async planId => { const response = await fetch(`${API}/subscriptions`, { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({userId: 1, planId}) }); setMessage(response.ok ? 'Suscripción activada correctamente.' : 'No se pudo activar la suscripción.'); };
  return <div className="app">
    <header><span className="logo">♫</span><strong>Sonora</strong><nav><a href="#discover">Descubrir</a><a href="#plans">Planes</a><a href="#playlists">Playlists</a></nav><button className="avatar">DU</button></header>
    <main><section className="hero" id="discover"><div><p className="eyebrow">TU MÚSICA, TU MOMENTO</p><h1>Encuentra el ritmo<br/><em>que te define.</em></h1><p className="lead">Descubre millones de canciones, crea tus playlists y disfruta música sin límites.</p><form onSubmit={search}><input value={query} onChange={e => setQuery(e.target.value)} placeholder="Buscar canciones, artistas o álbumes"/><button>Buscar</button></form></div><div className="hero-art">♫</div></section>
    {message && <div className="notice">{message}</div>}<section><div className="section-title"><div><p className="eyebrow">PARA TI</p><h2>Catálogo destacado</h2></div><span>{songs.length} canciones</span></div><div className="songs">{songs.map(song => <article className="song" key={song.id}><img src={song.coverUrl} alt=""/><div><h3>{song.title}</h3><p>{song.artist} · {song.album}</p><small>{song.genre} · {Math.floor(song.durationSeconds / 60)}:{String(song.durationSeconds % 60).padStart(2,'0')}</small></div><button className="play" aria-label={`Reproducir ${song.title}`}>▶</button></article>)}</div></section>
    <section id="plans" className="plans"><div className="section-title"><div><p className="eyebrow">SIN INTERRUPCIONES</p><h2>Elige tu plan</h2></div></div><div className="plan-grid">{plans.map(plan => <article className={plan.name === 'Premium' ? 'plan featured' : 'plan'} key={plan.id}><p className="eyebrow">{plan.name.toUpperCase()}</p><h3>{plan.price === 0 ? 'Gratis' : `$${plan.price.toFixed(2)} / mes`}</h3><p>{plan.description}</p><button onClick={() => subscribe(plan.id)}>Elegir plan</button></article>)}</div></section></main><footer>© 2026 Sonora · Plataforma de música</footer></div>;
}
createRoot(document.getElementById('root')).render(<App />);
