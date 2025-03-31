class Graph:
    def __init__(self):
        self.graph = {}
    def add_edge(self, u, v, weight):
        if u not in self.graph:
            self.graph[u] = []
        if v not in self.graph: 
            self.graph[v] = []
        self.graph[u].append((v, weight))
        self.graph[v].append((u, weight))

    def dijkstra(self, start):
        distances = {vertex: float('infinity') for vertex in self.graph}
        distances[start] = 0
        visited = set()
        while len(visited) < len(self.graph):
            current_vertex = None
            for vertex in distances:
                if vertex not in visited:
                    if current_vertex is None or distances[vertex] < distances[current_vertex]:
                        current_vertex = vertex
            if distances[current_vertex] == float('infinity'):
                break
            visited.add(current_vertex)
            for neighbor, weight in self.graph[current_vertex]:
                if neighbor not in visited:
                    new_distance = distances[current_vertex] + weight
                    if new_distance < distances[neighbor]:
                        distances[neighbor] = new_distance
        return distances
if __name__ == "__main__":
    g = Graph()
    g.add_edge('A', 'B', 1)
    g.add_edge('A', 'C', 4)
    g.add_edge('B', 'C', 2)
    g.add_edge('B', 'D', 5)
    g.add_edge('C', 'D', 1)
    start_vertex = 'A'
    distances = g.dijkstra(start_vertex)
    print(f"Расстояния от вершины '{start_vertex}':")
    for vertex, distance in distances.items():
        print(f"До '{vertex}': {distance}")
