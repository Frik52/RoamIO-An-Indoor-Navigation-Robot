import pygame
import sys
import heapq
import time
import random

# Initialize pygame
pygame.init()

# Grid size and screen dimensions
ROWS, COLS = 20, 20
CELL_SIZE = 30
WIDTH, HEIGHT = COLS * CELL_SIZE, ROWS * CELL_SIZE

# Setup display
WIN = pygame.display.set_mode((WIDTH, HEIGHT))
pygame.display.set_caption("🤖 Autonomous Robot with Dynamic Obstacles, Replanning, Goal Changing, and Obstacle Removal")

# Define colors
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
GREY = (220, 220, 220)
RED = (255, 0, 0)
GREEN = (0, 255, 0)
BLUE = (0, 0, 255)
PURPLE = (160, 32, 240)

# Load emoji font
pygame.font.init()
font = pygame.font.SysFont("Segoe UI Emoji", 24)

# Node class
class Node:
    def __init__(self, row, col):
        self.row = row
        self.col = col
        self.x = col * CELL_SIZE
        self.y = row * CELL_SIZE
        self.color = WHITE
        self.neighbors = []
        self.g = float("inf")
        self.h = 0
        self.f = 0
        self.parent = None
        self.is_obstacle = False

    def get_pos(self):
        return self.row, self.col

    def reset(self):
        self.color = WHITE
        self.is_obstacle = False
        self.g = float("inf")
        self.h = 0
        self.f = 0
        self.parent = None

    def make_start(self): self.color = GREEN
    def make_end(self): self.color = BLUE
    def make_closed(self): self.color = RED
    def make_open(self): self.color = PURPLE
    def make_path(self): self.color = GREY
    def make_obstacle(self):
        self.is_obstacle = True
        self.color = BLACK

    def remove_obstacle(self):
        self.is_obstacle = False
        self.color = WHITE

    def draw(self, win):
        pygame.draw.rect(win, self.color, (self.x, self.y, CELL_SIZE, CELL_SIZE))
        if self.is_obstacle:
            pygame.draw.circle(win, RED, (self.x + CELL_SIZE // 2, self.y + CELL_SIZE // 2), CELL_SIZE // 3)

    def update_neighbors(self, grid):
        self.neighbors = []
        for dr, dc in [(-1,0), (1,0), (0,-1), (0,1)]:
            r, c = self.row + dr, self.col + dc
            if 0 <= r < ROWS and 0 <= c < COLS and not grid[r][c].is_obstacle:
                self.neighbors.append(grid[r][c])

# Heuristic
def heuristic(a, b):
    return abs(a.row - b.row) + abs(a.col - b.col)

# Reconstruct path
def reconstruct_path(came_from, current, draw, robot, grid, end):
    path = []
    while current in came_from:
        current = came_from[current]
        path.append(current)
    path.reverse()

    for node in path:
        if node.color != GREEN and node.color != BLUE:
            node.make_path()
        draw()
        robot.move_to(node)
        time.sleep(0.05)

        # Randomly place a dynamic obstacle with small probability
        if random.random() < 0.05:
            place_random_obstacle(grid, robot, end)
            for row in grid:
                for n in row:
                    n.update_neighbors(grid)
            # Re-plan from current position
            replan(draw, grid, robot, end)
            return

# A* algorithm with auto re-planning
def a_star(draw, grid, start, end, robot):
    count = 0
    open_set = []
    heapq.heappush(open_set, (0, count, start))
    came_from = {}
    start.g = 0
    start.f = heuristic(start, end)

    open_set_hash = {start}

    while open_set:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit(); sys.exit()

        current = heapq.heappop(open_set)[2]
        open_set_hash.remove(current)

        if current == end:
            reconstruct_path(came_from, end, draw, robot, grid, end)
            return True

        for neighbor in current.neighbors:
            temp_g = current.g + 1

            if temp_g < neighbor.g:
                came_from[neighbor] = current
                neighbor.g = temp_g
                neighbor.h = heuristic(neighbor, end)
                neighbor.f = neighbor.g + neighbor.h
                if neighbor not in open_set_hash:
                    count += 1
                    heapq.heappush(open_set, (neighbor.f, count, neighbor))
                    open_set_hash.add(neighbor)
                    neighbor.make_open()

        draw()
        if current != start:
            current.make_closed()

    return False

# Re-plan function
def replan(draw, grid, robot, end):
    for row in grid:
        for node in row:
            node.g = float("inf")
            node.h = 0
            node.f = 0
            node.parent = None
    a_star(draw, grid, robot.node, end, robot)

# Place random obstacle avoiding robot and end
def place_random_obstacle(grid, robot, end):
    while True:
        r = random.randint(0, ROWS-1)
        c = random.randint(0, COLS-1)
        node = grid[r][c]
        if node != robot.node and node != end and not node.is_obstacle:
            node.make_obstacle()
            break

# Create grid
def make_grid():
    return [[Node(i, j) for j in range(COLS)] for i in range(ROWS)]

# Draw grid lines
def draw_grid(win):
    for i in range(ROWS):
        pygame.draw.line(win, GREY, (0, i * CELL_SIZE), (WIDTH, i * CELL_SIZE))
    for j in range(COLS):
        pygame.draw.line(win, GREY, (j * CELL_SIZE, 0), (j * CELL_SIZE, HEIGHT))

# Draw everything
def draw(win, grid, robot):
    win.fill(WHITE)
    for row in grid:
        for node in row:
            node.draw(win)
    robot.draw(win)
    draw_grid(win)
    pygame.display.update()

# Convert mouse click to grid position
def get_clicked_pos(pos):
    y, x = pos
    return x // CELL_SIZE, y // CELL_SIZE

# Robot class
class Robot:
    def __init__(self, node):
        self.node = node

    def move_to(self, node):
        self.node = node

    def draw(self, win):
        text = font.render("🤖", True, (0, 0, 0))
        win.blit(text, (self.node.x + 4, self.node.y + 2))

# Main function
def main(win):
    grid = make_grid()
    start = grid[0][0]
    end = grid[ROWS - 1][COLS - 1]
    start.make_start()
    end.make_end()

    robot = Robot(start)

    running = True

    while running:
        for row in grid:
            for node in row:
                node.update_neighbors(grid)

        draw(win, grid, robot)

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
                pygame.quit(); sys.exit()

            if pygame.mouse.get_pressed()[0]:  # Left click to place obstacle
                pos = pygame.mouse.get_pos()
                row, col = get_clicked_pos(pos)
                node = grid[row][col]
                if node != start and node != end:
                    node.make_obstacle()

            if pygame.mouse.get_pressed()[2]:  # Right click to remove obstacle or set goal
                pos = pygame.mouse.get_pos()
                row, col = get_clicked_pos(pos)
                node = grid[row][col]
                if node.is_obstacle:
                    node.remove_obstacle()
                elif node != start:
                    # Change goal dynamically
                    end = node
                    for r in grid:
                        for n in r:
                            if n.color == BLUE:
                                n.color = WHITE
                    end.make_end()
                    replan(lambda: draw(win, grid, robot), grid, robot, end)

            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_SPACE:
                    for row in grid:
                        for node in row:
                            node.reset()
                    start = grid[0][0]
                    end = grid[ROWS - 1][COLS - 1]
                    start.make_start()
                    end.make_end()
                    robot.move_to(start)

                if event.key == pygame.K_RETURN:
                    for row in grid:
                        for node in row:
                            node.update_neighbors(grid)
                            node.g = float("inf")
                            node.h = 0
                            node.f = 0
                            node.parent = None
                    a_star(lambda: draw(win, grid, robot), grid, robot.node, end, robot)

        pygame.time.delay(50)

if __name__ == "__main__":
    main(WIN)
