const canvas = document.getElementById('game');
const ctx = canvas.getContext('2d');

const ui = {
  score: document.getElementById('score'),
  level: document.getElementById('level'),
  lives: document.getElementById('lives'),
  combo: document.getElementById('combo'),
  highScore: document.getElementById('highScore'),
  status: document.getElementById('status'),
  startBtn: document.getElementById('startBtn'),
  muteBtn: document.getElementById('muteBtn'),
  musicBtn: document.getElementById('musicBtn'),
};

const levels = [
  [0, 'Summer in Canada', 1.0],
  [250, 'Diamond Dust', 1.2],
  [500, 'Snow Flurry', 1.4],
  [900, 'Snow Storm', 1.7],
  [1400, 'Arctic Mayhem!', 2.0],
];

const state = {
  running: false,
  paused: false,
  muted: false,
  musicMuted: false,
  score: 0,
  lives: 3,
  combo: 1,
  comboTimer: 0,
  levelName: levels[0][1],
  enemySpeed: 70,
  spawnRate: 0.9,
  lastTime: 0,
  enemyCooldown: 0,
  bulletCooldown: 0,
  dashCooldown: 0,
  stars: [],
  particles: [],
  enemies: [],
  bullets: [],
  powerups: [],
  boss: null,
  highScore: Number(localStorage.getItem('snowflake-web-highscore') || 0),
};

const keys = new Set();
const touchState = { up: false, down: false, left: false, right: false };
const hero = { x: canvas.width / 2, y: canvas.height - 60, radius: 18, speed: 300, shield: 0 };

const audio = {
  ctx: null,
  master: null,
  music: null,
  musicTimer: null,
  musicStep: 0,
};

for (let i = 0; i < 140; i += 1) {
  state.stars.push({ x: Math.random() * canvas.width, y: Math.random() * canvas.height, z: Math.random() * 1.5 + 0.3 });
}

function ensureAudio() {
  if (!audio.ctx) {
    audio.ctx = new (window.AudioContext || window.webkitAudioContext)();
    audio.master = audio.ctx.createGain();
    audio.master.gain.value = 0.18;
    audio.master.connect(audio.ctx.destination);
    audio.music = audio.ctx.createGain();
    audio.music.gain.value = 0.05;
    audio.music.connect(audio.master);
  }
  if (audio.ctx.state === 'suspended') audio.ctx.resume();
}

function playSfx(freq, duration = 0.06, type = 'triangle', volume = 0.08) {
  if (state.muted) return;
  ensureAudio();
  const osc = audio.ctx.createOscillator();
  const gain = audio.ctx.createGain();
  osc.type = type;
  osc.frequency.value = freq;
  gain.gain.setValueAtTime(volume, audio.ctx.currentTime);
  gain.gain.exponentialRampToValueAtTime(0.001, audio.ctx.currentTime + duration);
  osc.connect(gain).connect(audio.master);
  osc.start();
  osc.stop(audio.ctx.currentTime + duration);
}

function startMusic() {
  ensureAudio();
  if (audio.musicTimer) return;
  const melody = [220, 247, 262, 247, 220, 196, 220, 262];
  audio.musicTimer = window.setInterval(() => {
    if (state.musicMuted || !state.running || state.paused) return;
    const freq = melody[audio.musicStep % melody.length];
    audio.musicStep += 1;
    const osc = audio.ctx.createOscillator();
    const gain = audio.ctx.createGain();
    osc.type = 'sine';
    osc.frequency.value = freq;
    gain.gain.value = 0.03;
    osc.connect(gain).connect(audio.music);
    osc.start();
    osc.stop(audio.ctx.currentTime + 0.24);
  }, 280);
}

function stopMusic() {
  if (audio.musicTimer) {
    clearInterval(audio.musicTimer);
    audio.musicTimer = null;
  }
}

function updateHud() {
  let active = levels[0];
  for (const level of levels) if (state.score >= level[0]) active = level;
  state.levelName = active[1];
  state.enemySpeed = 62 * active[2];
  state.spawnRate = 0.8 * active[2];

  ui.score.textContent = Math.floor(state.score);
  ui.level.textContent = state.levelName;
  ui.lives.textContent = state.lives;
  ui.combo.textContent = `x${state.combo.toFixed(1)}`;
  ui.highScore.textContent = state.highScore;
}

function resetGame() {
  ensureAudio();
  startMusic();
  Object.assign(state, {
    running: true,
    paused: false,
    score: 0,
    lives: 3,
    combo: 1,
    comboTimer: 0,
    enemySpeed: 70,
    spawnRate: 0.9,
    enemyCooldown: 0,
    bulletCooldown: 0,
    dashCooldown: 0,
    particles: [],
    enemies: [],
    bullets: [],
    powerups: [],
    boss: null,
  });
  hero.x = canvas.width / 2;
  hero.y = canvas.height - 60;
  hero.shield = 0;
  ui.status.textContent = 'Snowfall incoming. Survive and build combo!';
  playSfx(370, 0.08);
  updateHud();
}

function spawnEnemy(fromBoss = false, x = null, y = -30) {
  const fast = Math.random() < 0.22;
  state.enemies.push({
    x: x ?? Math.random() * (canvas.width - 70) + 35,
    y,
    r: fast ? 10 : 14,
    hp: fast ? 1 : 2,
    speed: state.enemySpeed * (fast ? 1.65 : 1),
    wobble: Math.random() * Math.PI * 2,
    fromBoss,
  });
}

function spawnBoss() {
  state.boss = { x: canvas.width / 2, y: 88, hp: 70, drift: 0, fireCooldown: 0 };
  ui.status.textContent = '⚠️ Glacier Titan has entered the arena!';
  playSfx(120, 0.25, 'square', 0.14);
}

function shoot() {
  if (!state.running || state.paused || state.bulletCooldown > 0) return;
  state.bullets.push({ x: hero.x - 8, y: hero.y - 12, vy: -520, r: 4 });
  state.bullets.push({ x: hero.x + 8, y: hero.y - 12, vy: -520, r: 4 });
  playSfx(540, 0.05);
  state.bulletCooldown = 0.11;
}

function burst(x, y, color, amount = 14) {
  for (let i = 0; i < amount; i += 1) {
    const a = Math.random() * Math.PI * 2;
    const v = Math.random() * 190 + 40;
    state.particles.push({ x, y, vx: Math.cos(a) * v, vy: Math.sin(a) * v, life: 0.8, color });
  }
}

function intersects(a, b, pad = 0) {
  const dx = a.x - b.x;
  const dy = a.y - b.y;
  return Math.hypot(dx, dy) < (a.r || a.radius) + (b.r || b.radius) + pad;
}

function setGameOver() {
  state.running = false;
  state.highScore = Math.max(state.highScore, Math.floor(state.score));
  localStorage.setItem('snowflake-web-highscore', String(state.highScore));
  ui.status.textContent = `Game over. Final score ${Math.floor(state.score)}.`;
  playSfx(100, 0.25, 'sawtooth', 0.14);
  updateHud();
}

function applyInput(dt) {
  const left = keys.has('arrowleft') || keys.has('a') || touchState.left;
  const right = keys.has('arrowright') || keys.has('d') || touchState.right;
  const up = keys.has('arrowup') || keys.has('w') || touchState.up;
  const down = keys.has('arrowdown') || keys.has('s') || touchState.down;

  if (left) hero.x -= hero.speed * dt;
  if (right) hero.x += hero.speed * dt;
  if (up) hero.y -= hero.speed * dt;
  if (down) hero.y += hero.speed * dt;

  hero.x = Math.max(hero.radius, Math.min(canvas.width - hero.radius, hero.x));
  hero.y = Math.max(hero.radius, Math.min(canvas.height - hero.radius, hero.y));
}

function update(dt) {
  if (!state.running || state.paused) return;

  state.bulletCooldown = Math.max(0, state.bulletCooldown - dt);
  state.enemyCooldown -= dt;
  state.dashCooldown = Math.max(0, state.dashCooldown - dt);
  state.comboTimer -= dt;
  if (state.comboTimer <= 0) state.combo = Math.max(1, state.combo - dt * 0.6);

  applyInput(dt);

  if (state.enemyCooldown <= 0) {
    spawnEnemy();
    state.enemyCooldown = Math.max(0.15, 0.95 / state.spawnRate);
  }

  if (!state.boss && state.score >= 1200) spawnBoss();

  for (const b of state.bullets) b.y += b.vy * dt;
  state.bullets = state.bullets.filter((b) => b.y > -30 && !b.dead);

  for (const e of state.enemies) {
    e.y += e.speed * dt;
    e.x += Math.sin((e.y + e.wobble) / 18) * 34 * dt;

    // Bugfix: missed enemies no longer remove lives; only direct collisions damage hero.
    if (e.y > canvas.height + 30) e.dead = true;

    if (!e.dead && intersects(e, hero, -4)) {
      e.dead = true;
      if (hero.shield > 0) {
        hero.shield -= 1;
        burst(hero.x, hero.y, '#f4d466', 16);
        playSfx(280, 0.07, 'square');
      } else {
        state.lives -= 1;
        state.combo = 1;
        burst(hero.x, hero.y, '#ff8a8a', 22);
        playSfx(160, 0.15, 'sawtooth', 0.13);
      }
      if (state.lives <= 0) setGameOver();
    }
  }

  for (const e of state.enemies) {
    for (const b of state.bullets) {
      if (!e.dead && !b.dead && intersects(e, b)) {
        b.dead = true;
        e.hp -= 1;
        if (e.hp <= 0) {
          e.dead = true;
          state.score += 10 * state.combo;
          state.combo = Math.min(4, state.combo + 0.1);
          state.comboTimer = 2.2;
          burst(e.x, e.y, '#8bd9ff');
          playSfx(650, 0.04, 'triangle', 0.06);
          if (Math.random() < 0.08) {
            state.powerups.push({ x: e.x, y: e.y, r: 10, type: Math.random() < 0.5 ? 'shield' : 'multishot' });
          }
        }
      }
    }
  }

  for (const p of state.powerups) {
    p.y += 90 * dt;
    if (intersects(p, hero, 2)) {
      p.dead = true;
      if (p.type === 'shield') {
        hero.shield = Math.min(3, hero.shield + 1);
        ui.status.textContent = `Shield charged (${hero.shield})`;
      } else {
        state.score += 100;
        state.combo = Math.min(4, state.combo + 0.4);
        ui.status.textContent = 'Multishot bonus! +100';
      }
      burst(p.x, p.y, '#f4d466', 20);
      playSfx(720, 0.09, 'triangle', 0.08);
    }
  }

  if (state.boss) {
    const boss = state.boss;
    boss.drift += dt;
    boss.x = canvas.width / 2 + Math.sin(boss.drift) * 250;

    for (const b of state.bullets) {
      if (!b.dead && intersects({ x: boss.x, y: boss.y, r: 46 }, b)) {
        b.dead = true;
        boss.hp -= 1;
        burst(b.x, b.y, '#ffca6b', 4);
        if (boss.hp <= 0) {
          state.score += 1500;
          burst(boss.x, boss.y, '#ffffff', 100);
          state.boss = null;
          ui.status.textContent = 'Boss destroyed! Endless mode unlocked.';
          playSfx(920, 0.2, 'triangle', 0.12);
          break;
        }
      }
    }

    if (state.boss) {
      boss.fireCooldown -= dt;
      if (boss.fireCooldown <= 0) {
        spawnEnemy(true, boss.x, boss.y + 40);
        boss.fireCooldown = 0.35;
      }
    }
  }

  for (const pt of state.particles) {
    pt.x += pt.vx * dt;
    pt.y += pt.vy * dt;
    pt.life -= dt;
    pt.vx *= 0.98;
    pt.vy *= 0.98;
  }

  state.particles = state.particles.filter((p) => p.life > 0);
  state.enemies = state.enemies.filter((e) => !e.dead);
  state.powerups = state.powerups.filter((p) => !p.dead && p.y < canvas.height + 30);

  state.score += dt * 3;
  updateHud();
}

function draw() {
  ctx.clearRect(0, 0, canvas.width, canvas.height);
  const bg = ctx.createLinearGradient(0, 0, 0, canvas.height);
  bg.addColorStop(0, '#09162e');
  bg.addColorStop(1, '#03060f');
  ctx.fillStyle = bg;
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  for (const star of state.stars) {
    star.y += star.z;
    if (star.y > canvas.height) {
      star.y = 0;
      star.x = Math.random() * canvas.width;
    }
    ctx.fillStyle = `rgba(180,220,255,${0.25 + star.z / 2})`;
    ctx.fillRect(star.x, star.y, star.z * 1.4, star.z * 1.4);
  }

  if (hero.shield > 0) {
    ctx.beginPath();
    ctx.arc(hero.x, hero.y, 26, 0, Math.PI * 2);
    ctx.strokeStyle = 'rgba(125,241,255,0.65)';
    ctx.lineWidth = 3;
    ctx.stroke();
  }

  ctx.beginPath();
  ctx.moveTo(hero.x, hero.y - 18);
  ctx.lineTo(hero.x - 16, hero.y + 18);
  ctx.lineTo(hero.x + 16, hero.y + 18);
  ctx.closePath();
  ctx.fillStyle = '#72d9ff';
  ctx.shadowBlur = 24;
  ctx.shadowColor = '#84e2ff';
  ctx.fill();
  ctx.shadowBlur = 0;

  for (const b of state.bullets) {
    ctx.fillStyle = '#ffe684';
    ctx.fillRect(b.x - 2, b.y - 8, 4, 12);
  }

  for (const e of state.enemies) {
    ctx.beginPath();
    for (let i = 0; i < 8; i += 1) {
      const a = (Math.PI * 2 * i) / 8;
      const r = i % 2 === 0 ? e.r : e.r * 0.55;
      const px = e.x + Math.cos(a) * r;
      const py = e.y + Math.sin(a) * r;
      if (i === 0) ctx.moveTo(px, py);
      else ctx.lineTo(px, py);
    }
    ctx.closePath();
    ctx.fillStyle = e.fromBoss ? '#f6b2b2' : '#d5ecff';
    ctx.shadowBlur = 15;
    ctx.shadowColor = '#eaf7ff';
    ctx.fill();
    ctx.shadowBlur = 0;
  }

  for (const p of state.powerups) {
    ctx.beginPath();
    ctx.arc(p.x, p.y, p.r, 0, Math.PI * 2);
    ctx.fillStyle = p.type === 'shield' ? '#66f3de' : '#ffd87f';
    ctx.fill();
  }

  if (state.boss) {
    const b = state.boss;
    ctx.beginPath();
    ctx.arc(b.x, b.y, 46, 0, Math.PI * 2);
    ctx.fillStyle = '#9cc8ff';
    ctx.fill();
    ctx.fillStyle = '#001630';
    ctx.fillRect(b.x - 40, b.y - 60, 80, 8);
    ctx.fillStyle = '#fe6d6d';
    ctx.fillRect(b.x - 40, b.y - 60, 80 * (b.hp / 70), 8);
  }

  for (const pt of state.particles) {
    ctx.fillStyle = pt.color;
    ctx.globalAlpha = Math.max(pt.life, 0);
    ctx.fillRect(pt.x, pt.y, 3, 3);
  }
  ctx.globalAlpha = 1;

  if (state.paused) {
    ctx.fillStyle = 'rgba(0,0,0,0.45)';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    ctx.fillStyle = '#e8f4ff';
    ctx.font = 'bold 44px Segoe UI';
    ctx.fillText('PAUSED', canvas.width / 2 - 90, canvas.height / 2);
  }
}

function frame(ts) {
  if (!state.lastTime) state.lastTime = ts;
  const dt = Math.min(0.033, (ts - state.lastTime) / 1000);
  state.lastTime = ts;
  update(dt);
  draw();
  requestAnimationFrame(frame);
}

function triggerDash() {
  if (!state.running || state.paused || state.dashCooldown > 0) return;
  const dir = (keys.has('arrowleft') || keys.has('a') || touchState.left) ? -1 : 1;
  hero.x = Math.max(hero.radius, Math.min(canvas.width - hero.radius, hero.x + dir * 95));
  state.dashCooldown = 1.5;
  burst(hero.x, hero.y, '#66f3de', 10);
  playSfx(430, 0.08, 'square', 0.09);
}

window.addEventListener('keydown', (e) => {
  const key = e.key.toLowerCase();
  keys.add(key);
  if (key === ' ') {
    e.preventDefault();
    shoot();
  }
  if (key === 'p') {
    state.paused = !state.paused;
    ui.status.textContent = state.paused ? 'Paused' : 'Back in action';
  }
  if (key === 'shift') triggerDash();
});

window.addEventListener('keyup', (e) => keys.delete(e.key.toLowerCase()));

for (const button of document.querySelectorAll('[data-touch]')) {
  const action = button.dataset.touch;
  const holdable = ['up', 'down', 'left', 'right', 'shoot'].includes(action);

  const start = (event) => {
    event.preventDefault();
    ensureAudio();
    if (['up', 'down', 'left', 'right'].includes(action)) touchState[action] = true;
    if (action === 'shoot') shoot();
    if (action === 'dash') triggerDash();
    if (action === 'pause') state.paused = !state.paused;
  };

  const end = (event) => {
    if (event) event.preventDefault();
    if (['up', 'down', 'left', 'right'].includes(action)) touchState[action] = false;
  };

  button.addEventListener('pointerdown', start);
  if (holdable) {
    button.addEventListener('pointerup', end);
    button.addEventListener('pointercancel', end);
    button.addEventListener('pointerleave', end);
  }
}

canvas.addEventListener('pointerdown', (event) => {
  event.preventDefault();
  ensureAudio();
  shoot();
});

ui.startBtn.addEventListener('click', () => resetGame());
ui.muteBtn.addEventListener('click', () => {
  state.muted = !state.muted;
  ui.muteBtn.textContent = state.muted ? 'Unmute SFX' : 'Mute SFX';
});
ui.musicBtn.addEventListener('click', () => {
  state.musicMuted = !state.musicMuted;
  ui.musicBtn.textContent = state.musicMuted ? 'Unmute Music' : 'Mute Music';
});

updateHud();
requestAnimationFrame(frame);
