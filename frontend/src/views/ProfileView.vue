<template>
  <v-card
    class="mx-auto"
    max-width="1000"
  >
    <v-card-item>
    </v-card-item>

    <v-card-text class="py-0">
      <v-row align="center" no-gutters>
        <v-col
          class="text-h2"
          cols="6"
        >
          {{ $store.state.account.requests.stats.data.nickname }}
        </v-col>

        <v-col class="text-right" cols="6">
          <v-icon
            color="error"
            icon="mdi-ferry"
            size="88"
          ></v-icon>
        </v-col>
      </v-row>
    </v-card-text>

    <div class="d-flex py-3 justify-space-between">
      <v-list-item
        color="amber-accent-2"
        density="compact"
        prepend-icon="mdi-stairs-up"
      >
        <v-list-item-subtitle>{{ $store.state.account.requests.stats.data.eloRating }} ELO</v-list-item-subtitle>
      </v-list-item>

      <v-list-item
        color="green"
        density="compact"
        prepend-icon="mdi-trending-up"
      >
        <v-list-item-subtitle>{{ $store.state.account.requests.stats.data.wins }}</v-list-item-subtitle>
      </v-list-item>

      <v-list-item
        color="red"
        density="compact"
        prepend-icon="mdi-trending-down"
      >
        <v-list-item-subtitle>{{ $store.state.account.requests.stats.data.losses }}</v-list-item-subtitle>
      </v-list-item>

      <v-list-item
        v-if="Math.round($store.state.account.requests.stats.data.wins / $store.state.account.requests.stats.data.battles * 100) <= 50"
        color="red"
        density=""
        prepend-icon="mdi-arrow-down"
      >
        <v-list-item-subtitle>
          {{ Math.round($store.state.account.requests.stats.data.wins / $store.state.account.requests.stats.data.battles * 100)
          }}%
        </v-list-item-subtitle>
      </v-list-item>

      <v-list-item
        v-if="Math.round($store.state.account.requests.stats.data.wins / $store.state.account.requests.stats.data.battles * 100) > 50"
        color="green"
        density="compact"
        prepend-icon="mdi-arrow-up"
      >
        <v-list-item-subtitle>
          {{ Math.round($store.state.account.requests.stats.data.wins / $store.state.account.requests.stats.data.battles * 100)
          }}%
        </v-list-item-subtitle>
      </v-list-item>
    </div>

    <v-expand-transition>
      <div v-if="expand">
        <div style="height: 600px !important; overflow-y: auto">
          <v-table fixed-header>
            <thead>
            <tr>
              <th class="text-left">
                Дата
              </th>
              <th class="text-left">
                Результат
              </th>
              <th class="text-left">
                ELO Delta
              </th>
            </tr>
            </thead>
            <tbody>
            <tr
              v-for="item in $store.state.account.requests.stats.data.stats"
              :key="item.name"
            >
              <td>{{ item.date }}</td>
              <td>
                <v-chip v-if="item.isWin" color="green">
                  Перемога
                </v-chip>

                <v-chip v-if="!item.isWin" color="red">
                  Поразка
                </v-chip>
              </td>
              <td>
                {{ item.eloDelta }}
              </td>
            </tr>
            </tbody>
          </v-table>
        </div>
      </div>
    </v-expand-transition>

    <v-divider></v-divider>

    <v-card-actions>
      <v-btn @click="expand = !expand">
        {{ !expand ? "Детальніше" : "Приховати" }}
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>


export default ({
  data: () => ({
    expand: false
  }),
  mounted() {
    this.$store.dispatch("account/getStats");
  }
});
</script>
