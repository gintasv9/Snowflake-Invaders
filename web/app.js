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
const hero = {
  x: canvas.width / 2,
  y: canvas.height - 60,
  radius: 18,
  speed: 290,
  color: '#72d9ff',
  shield: 0,
};

for (let i = 0; i < 140; i += 1) {
  state.stars.push({
    x: Math.random() * canvas.width,
    y: Math.random() * canvas.height,
    z: Math.random() * 1.5 + 0.3,
  });
}

function resetGame() {
  state.running = true;
  state.paused = false;
  state.score = 0;
  state.lives = 3;
  state.combo = 1;
  state.comboTimer = 0;
  state.enemySpeed = 70;
  state.spawnRate = 0.9;
  state.enemyCooldown = 0;
  state.bulletCooldown = 0;
  state.dashCooldown = 0;
  state.particles = [];
  state.enemies = [];
  state.bullets = [];
  state.powerups = [];
  state.boss = null;
  hero.x = canvas.width / 2;
  hero.y = canvas.height - 60;
  hero.shield = 0;
  ui.status.textContent = 'Survive the snowstorm and stack combos!';
  updateHud();
}

function updateHud() {
  let active = levels[0];
  for (const lvl of levels) if (state.score >= lvl[0]) active = lvl;

  state.levelName = active[1];
  state.enemySpeed = 60 * active[2];
  state.spawnRate = 0.85 * active[2];

  ui.score.textContent = Math.floor(state.score);
  ui.level.textContent = state.levelName;
  ui.lives.textContent = state.lives;
  ui.combo.textContent = `x${state.combo.toFixed(1)}`;
  ui.highScore.textContent = state.highScore;
}

function spawnEnemy() {
  const isFast = Math.random() < 0.22;
  state.enemies.push({
    x: Math.random() * (canvas.width - 70) + 35,
    y: -30,
    r: isFast ? 10 : 14,
    hp: isFast ? 1 : 2,
    speed: state.enemySpeed * (isFast ? 1.8 : 1),
    wobble: Math.random() * Math.PI * 2,
  });
}

function spawnBoss() {
  state.boss = { x: canvas.width / 2, y: 88, hp: 80, drift: 0, fireCooldown: 0 };
  ui.status.textContent = '⚠️ Raid boss appeared: Glacier Titan!';
}

function shoot() {
  if (state.bulletCooldown > 0) return;
  state.bullets.push({ x: hero.x - 8, y: hero.y - 12, vy: -520, r: 4 });
  state.bullets.push({ x: hero.x + 8, y: hero.y - 12, vy: -520, r: 4 });
  if (!state.muted) clickSound(540, 0.02);
  state.bulletCooldown = 0.11;
}

function clickSound(freq, duration) {
  const actx = new (window.AudioContext || window.webkitAudioContext)();
  const osc = actx.createOscillator();
  const gain = actx.createGain();
  osc.type = 'triangle';
  osc.frequency.value = freq;
  gain.gain.value = 0.02;
  osc.connect(gain).connect(actx.destination);
  osc.start();
  osc.stop(actx.currentTime + duration);
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
  ui.status.textContent = `Game over. Final score ${Math.floor(state.score)}. Tap Start to play again.`;
  updateHud();
}

function update(dt) {
  if (!state.running || state.paused) return;

  state.bulletCooldown = Math.max(0, state.bulletCooldown - dt);
  state.enemyCooldown -= dt;
  state.dashCooldown = Math.max(0, state.dashCooldown - dt);
  state.comboTimer -= dt;
  if (state.comboTimer <= 0) state.combo = Math.max(1, state.combo - dt * 0.6);

  if (keys.has('arrowleft') || keys.has('a')) hero.x -= hero.speed * dt;
  if (keys.has('arrowright') || keys.has('d')) hero.x += hero.speed * dt;
  if (keys.has('arrowup') || keys.has('w')) hero.y -= hero.speed * dt;
  if (keys.has('arrowdown') || keys.has('s')) hero.y += hero.speed * dt;

  hero.x = Math.max(hero.radius, Math.min(canvas.width - hero.radius, hero.x));
  hero.y = Math.max(hero.radius, Math.min(canvas.height - hero.radius, hero.y));

  if (state.enemyCooldown <= 0) {
    spawnEnemy();
    state.enemyCooldown = Math.max(0.12, 0.85 / state.spawnRate);
  }

  if (!state.boss && state.score >= 1200) spawnBoss();

  for (const b of state.bullets) b.y += b.vy * dt;
  state.bullets = state.bullets.filter((b) => b.y > -30);

  for (const e of state.enemies) {
    e.y += e.speed * dt;
    e.x += Math.sin((e.y + e.wobble) / 18) * 36 * dt;

    if (e.y > canvas.height + 20) {
      e.dead = true;
      state.lives -= 1;
      state.combo = 1;
      if (state.lives <= 0) setGameOver();
    }

    if (intersects(e, hero, -5)) {
      e.dead = true;
      if (hero.shield > 0) {
        hero.shield -= 1;
        burst(hero.x, hero.y, '#f4d466', 16);
      } else {
        state.lives -= 1;
        burst(hero.x, hero.y, '#ff8a8a', 22);
        state.combo = 1;
      }
      if (state.lives <= 0) setGameOver();
    }
  }

  for (const e of state.enemies) {
    for (const b of state.bullets) {
      if (!e.dead && intersects(e, b)) {
        b.dead = true;
        e.hp -= 1;
        if (e.hp <= 0) {
          e.dead = true;
          state.score += 10 * state.combo;
          state.combo = Math.min(4, state.combo + 0.1);
          state.comboTimer = 2.4;
          burst(e.x, e.y, '#8bd9ff');
          if (Math.random() < 0.08) {
            state.powerups.push({
              x: e.x,
              y: e.y,
              r: 10,
              type: Math.random() < 0.5 ? 'shield' : 'multishot',
            });
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
    }
  }

  if (state.boss) {
    const boss = state.boss;
    boss.drift += dt;
    boss.x = canvas.width / 2 + Math.sin(boss.drift) * 280;

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
        }
      }
    }

    boss.fireCooldown -= dt;
    if (boss.fireCooldown <= 0) {
      state.enemies.push({ x: boss.x, y: boss.y + 45, r: 11, hp: 1, speed: state.enemySpeed * 1.5, wobble: Math.random() * 3 });
      boss.fireCooldown = 0.2;
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
  state.bullets = state.bullets.filter((b) => !b.dead);
  state.powerups = state.powerups.filter((p) => !p.dead && p.y < canvas.height + 30);

  state.score += dt * 3;
  updateHud();
}

function draw() {
  ctx.clearRect(0, 0, canvas.width, canvas.height);

  const grad = ctx.createLinearGradient(0, 0, 0, canvas.height);
  grad.addColorStop(0, '#09162e');
  grad.addColorStop(1, '#03060f');
  ctx.fillStyle = grad;
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
  ctx.fillStyle = hero.color;
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
    ctx.fillStyle = '#d5ecff';
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
    ctx.fillRect(b.x - 40, b.y - 60, 80 * (b.hp / 80), 8);
  }

  for (const pt of state.particles) {
    ctx.fillStyle = pt.color;
    ctx.globalAlpha = Math.max(pt.life, 0);
    ctx.fillRect(pt.x, pt.y, 3, 3);
  }
  ctx.globalAlpha = 1;

  if (state.paused) {
    ctx.fillStyle = 'rgba(0,0,0,0.5)';
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
  if (key === 'shift' && state.dashCooldown <= 0) {
    const dir = keys.has('arrowleft') || keys.has('a') ? -1 : 1;
    hero.x += dir * 95;
    state.dashCooldown = 1.5;
    burst(hero.x, hero.y, '#66f3de', 10);
  }
});

window.addEventListener('keyup', (e) => keys.delete(e.key.toLowerCase()));

ui.startBtn.addEventListener('click', () => resetGame());
ui.muteBtn.addEventListener('click', () => {
  state.muted = !state.muted;
  ui.muteBtn.textContent = state.muted ? 'Unmute SFX' : 'Mute SFX';
});

updateHud();
requestAnimationFrame(frame);
